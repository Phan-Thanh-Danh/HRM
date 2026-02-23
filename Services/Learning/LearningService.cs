using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.Learning;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Learning
{
    public interface ILearningService
    {
        Task<List<CourseVM>> GetCoursesAsync();
        Task<CourseVM?> GetCourseByIdAsync(int id);
        Task CreateCourseAsync(CourseVM courseVM);
        Task CreateLessonAsync(LessonVM lessonVM);
        Task<List<LessonVM>> GetLessonsByCourseIdAsync(int courseId);
        
        // Student/Employee methods
        Task EnrollAsync(int employeeId, int courseId);
        Task<List<CourseVM>> GetMyCoursesAsync(int employeeId);
        Task UpdateProgressAsync(int employeeId, int courseId, double progress);
    }

    public class LearningService : ILearningService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LearningService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateCourseAsync(CourseVM courseVM)
        {
            var course = _mapper.Map<Course>(courseVM);
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task CreateLessonAsync(LessonVM lessonVM)
        {
            var lesson = _mapper.Map<Lesson>(lessonVM);
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task EnrollAsync(int employeeId, int courseId)
        {
            var exists = await _context.EmployeeCourses
                .AnyAsync(ec => ec.EmployeeId == employeeId && ec.CourseId == courseId);
            
            if (!exists)
            {
                var enrollment = new EmployeeCourse
                {
                    EmployeeId = employeeId,
                    CourseId = courseId,
                    Status = CourseStatus.InProgress,
                    EnrolledDate = DateTime.UtcNow,
                    Progress = 0
                };
                _context.EmployeeCourses.Add(enrollment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CourseVM?> GetCourseByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Lessons)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            var vm = _mapper.Map<CourseVM>(course);
            if (course != null)
            {
                vm.LessonsCount = course.Lessons.Count;
            }
            return vm;
        }

        public async Task<List<CourseVM>> GetCoursesAsync()
        {
            var courses = await _context.Courses
                .Include(c => c.Lessons)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            
            var vms = _mapper.Map<List<CourseVM>>(courses);
            for (int i = 0; i < courses.Count; i++)
            {
                vms[i].LessonsCount = courses[i].Lessons.Count;
            }
            return vms;
        }

        public async Task<List<LessonVM>> GetLessonsByCourseIdAsync(int courseId)
        {
            var lessons = await _context.Lessons
                .Where(l => l.CourseId == courseId)
                .OrderBy(l => l.Order)
                .ToListAsync();
            return _mapper.Map<List<LessonVM>>(lessons);
        }

        public async Task<List<CourseVM>> GetMyCoursesAsync(int employeeId)
        {
            var enrollments = await _context.EmployeeCourses
                .Include(ec => ec.Course)
                .ThenInclude(c => c!.Lessons)
                .Where(ec => ec.EmployeeId == employeeId)
                .ToListAsync();

            var courseVMs = new List<CourseVM>();
            foreach (var enrollment in enrollments)
            {
                if (enrollment.Course != null)
                {
                    var vm = _mapper.Map<CourseVM>(enrollment.Course);
                    vm.EnrollmentStatus = enrollment.Status;
                    vm.Progress = enrollment.Progress;
                    vm.LessonsCount = enrollment.Course.Lessons.Count;
                    courseVMs.Add(vm);
                }
            }
            return courseVMs;
        }

        public async Task UpdateProgressAsync(int employeeId, int courseId, double progress)
        {
            var enrollment = await _context.EmployeeCourses
                .FirstOrDefaultAsync(ec => ec.EmployeeId == employeeId && ec.CourseId == courseId);
            
            if (enrollment != null)
            {
                enrollment.Progress = progress;
                if (progress >= 100)
                {
                    enrollment.Status = CourseStatus.Completed;
                    enrollment.CompletedDate = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
