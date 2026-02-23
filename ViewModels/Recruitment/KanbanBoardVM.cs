using HRM.Models;

namespace HRM.ViewModels.Recruitment
{
    public class ApplicationCardVM
    {
        public int Id { get; set; } // Application Id
        public string CandidateName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public DateTime AppliedDate { get; set; }
        public ApplicationStatus Status { get; set; }
    }

    public class KanbanBoardVM
    {
        public List<ApplicationCardVM> NewApplications { get; set; } = new List<ApplicationCardVM>();
        public List<ApplicationCardVM> ScreeningApplications { get; set; } = new List<ApplicationCardVM>();
        public List<ApplicationCardVM> InterviewApplications { get; set; } = new List<ApplicationCardVM>();
        public List<ApplicationCardVM> OfferApplications { get; set; } = new List<ApplicationCardVM>();
        public List<ApplicationCardVM> HiredApplications { get; set; } = new List<ApplicationCardVM>();
        public List<ApplicationCardVM> RejectedApplications { get; set; } = new List<ApplicationCardVM>();
    }
}
