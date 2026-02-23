using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.Recruitment;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Recruitment
{
    public interface IRecruitmentService
    {
        // Job Postings
        Task<List<JobPostingVM>> GetJobPostingsAsync();
        Task<JobPostingVM?> GetJobPostingByIdAsync(int id);
        Task CreateJobPostingAsync(JobPostingVM jobVM);
        Task UpdateJobPostingAsync(JobPostingVM jobVM);

        // Candidates & Applications
        Task ApplyAsync(CandidateVM candidateVM);
        Task<KanbanBoardVM> GetKanbanBoardAsync(int? jobPostingId);
        Task UpdateApplicationStatusAsync(int applicationId, ApplicationStatus status);
    }

    public class RecruitmentService : IRecruitmentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RecruitmentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task ApplyAsync(CandidateVM candidateVM)
        {
            // Simple check if candidate exists by email
            var candidate = await _context.Candidates.FirstOrDefaultAsync(c =>
                c.Email == candidateVM.Email
            );
            if (candidate == null)
            {
                candidate = _mapper.Map<Candidate>(candidateVM);
                _context.Candidates.Add(candidate);
                await _context.SaveChangesAsync();
            }

            if (candidateVM.JobPostingId.HasValue)
            {
                var application = new Application
                {
                    CandidateId = candidate.Id,
                    JobPostingId = candidateVM.JobPostingId.Value,
                    Status = ApplicationStatus.New,
                    AppliedDate = DateTime.UtcNow,
                };
                _context.Applications.Add(application);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateJobPostingAsync(JobPostingVM jobVM)
        {
            var job = _mapper.Map<JobPosting>(jobVM);
            _context.JobPostings.Add(job);
            await _context.SaveChangesAsync();
        }

        public async Task<List<JobPostingVM>> GetJobPostingsAsync()
        {
            var jobs = await _context
                .JobPostings.Include(j => j.Department)
                .Include(j => j.Applications)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            var vms = _mapper.Map<List<JobPostingVM>>(jobs);
            // Manually map counts if needed or use AutoMapper config
            for (int i = 0; i < jobs.Count; i++)
            {
                vms[i].ApplicationCount = jobs[i].Applications.Count;
            }
            return vms;
        }

        public async Task<JobPostingVM?> GetJobPostingByIdAsync(int id)
        {
            var job = await _context
                .JobPostings.Include(j => j.Department)
                .FirstOrDefaultAsync(j => j.Id == id);
            return _mapper.Map<JobPostingVM>(job);
        }

        public async Task<KanbanBoardVM> GetKanbanBoardAsync(int? jobPostingId)
        {
            var query = _context
                .Applications.Include(a => a.Candidate)
                .Include(a => a.JobPosting)
                .AsQueryable();

            if (jobPostingId.HasValue)
            {
                query = query.Where(a => a.JobPostingId == jobPostingId);
            }

            var applications = await query.ToListAsync();
            var cards = _mapper.Map<List<ApplicationCardVM>>(applications);

            return new KanbanBoardVM
            {
                NewApplications = cards.Where(c => c.Status == ApplicationStatus.New).ToList(),
                ScreeningApplications = cards
                    .Where(c => c.Status == ApplicationStatus.Screening)
                    .ToList(),
                InterviewApplications = cards
                    .Where(c => c.Status == ApplicationStatus.Interview)
                    .ToList(),
                OfferApplications = cards.Where(c => c.Status == ApplicationStatus.Offer).ToList(),
                HiredApplications = cards.Where(c => c.Status == ApplicationStatus.Hired).ToList(),
                RejectedApplications = cards
                    .Where(c => c.Status == ApplicationStatus.Rejected)
                    .ToList(),
            };
        }

        public async Task UpdateApplicationStatusAsync(int applicationId, ApplicationStatus status)
        {
            var app = await _context.Applications.FindAsync(applicationId);
            if (app != null)
            {
                app.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateJobPostingAsync(JobPostingVM jobVM)
        {
            var job = await _context.JobPostings.FindAsync(jobVM.Id);
            if (job != null)
            {
                _mapper.Map(jobVM, job);
                await _context.SaveChangesAsync();
            }
        }
    }
}
