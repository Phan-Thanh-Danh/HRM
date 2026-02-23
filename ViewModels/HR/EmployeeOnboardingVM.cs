namespace HRM.ViewModels.HR
{
    public class EmployeeOnboardingVM
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        
        public List<OnboardingTaskStatusVM> Tasks { get; set; } = new List<OnboardingTaskStatusVM>();
        
        public int CompletedCount => Tasks.Count(t => t.IsCompleted);
        public int TotalCount => Tasks.Count;
        public int ProgressPercentage => TotalCount == 0 ? 0 : (CompletedCount * 100 / TotalCount);
    }

    public class OnboardingTaskStatusVM
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsRequired { get; set; }
        
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Note { get; set; }
        
        // Use for form post
        public bool MarkedCompleted { get; set; }
    }
}
