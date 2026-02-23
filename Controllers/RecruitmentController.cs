using HRM.Services.Recruitment;
using HRM.Services.HR;
using HRM.ViewModels.Recruitment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HRM.Models;

namespace HRM.Controllers
{
    public class RecruitmentController : Controller
    {
        private readonly IRecruitmentService _recruitmentService;
        private readonly IDepartmentService _departmentService;

        public RecruitmentController(IRecruitmentService recruitmentService, IDepartmentService departmentService)
        {
            _recruitmentService = recruitmentService;
            _departmentService = departmentService;
        }

        // Public Job Board
        public async Task<IActionResult> Index()
        {
            var jobs = await _recruitmentService.GetJobPostingsAsync();
            var publishedJobs = jobs.Where(j => j.Status == JobStatus.Published && j.Deadline >= DateTime.Today).ToList();
            return View(publishedJobs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var job = await _recruitmentService.GetJobPostingByIdAsync(id);
            if (job == null) return NotFound();
            
            ViewBag.Job = job;
            return View(new CandidateVM { JobPostingId = job.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Apply(CandidateVM model)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload (Stub for now, or use IFormFile logic)
                if (model.CVFile != null)
                {
                    // Logic to save file would go here
                    // model.CVFilePath = ...
                }
                
                await _recruitmentService.ApplyAsync(model);
                return RedirectToAction(nameof(ApplySuccess));
            }
            
            // Reload job info if error
            if (model.JobPostingId.HasValue)
            {
                ViewBag.Job = await _recruitmentService.GetJobPostingByIdAsync(model.JobPostingId.Value);
            }
            return View("Details", model);
        }

        public IActionResult ApplySuccess()
        {
            return View();
        }

        // Internal Management
        [Authorize(Roles = "SuperAdmin,TalentAcquisition,HRManager")]
        public async Task<IActionResult> Manage()
        {
            var jobs = await _recruitmentService.GetJobPostingsAsync();
            return View(jobs);
        }

        [Authorize(Roles = "SuperAdmin,TalentAcquisition,HRManager")]
        public async Task<IActionResult> CreateJob()
        {
            ViewBag.Departments = await _departmentService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,TalentAcquisition,HRManager")]
        public async Task<IActionResult> CreateJob(JobPostingVM model)
        {
            if (ModelState.IsValid)
            {
                await _recruitmentService.CreateJobPostingAsync(model);
                return RedirectToAction(nameof(Manage));
            }
            ViewBag.Departments = await _departmentService.GetAllAsync();
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin,TalentAcquisition,HRManager")]
        public async Task<IActionResult> Kanban(int? jobId)
        {
            var jobs = await _recruitmentService.GetJobPostingsAsync();
            ViewBag.Jobs = jobs;
            ViewBag.CurrentJobId = jobId;

            var board = await _recruitmentService.GetKanbanBoardAsync(jobId);
            return View(board);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,TalentAcquisition,HRManager")]
        public async Task<IActionResult> UpdateStatus(int applicationId, ApplicationStatus status)
        {
            await _recruitmentService.UpdateApplicationStatusAsync(applicationId, status);
            return Ok();
        }
    }
}
