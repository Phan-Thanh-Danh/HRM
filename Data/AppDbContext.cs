using HRM.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRM.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<OnboardingTask> OnboardingTasks { get; set; }
        public DbSet<EmployeeOnboarding> EmployeeOnboardings { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        // C&B (Payroll)
        public DbSet<SalaryComponent> SalaryComponents { get; set; }
        public DbSet<SalaryFormula> SalaryFormulas { get; set; }
        public DbSet<PayrollPeriod> PayrollPeriods { get; set; }
        public DbSet<PayrollRecord> PayrollRecords { get; set; }

        // Talent Acquisition
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Interview> Interviews { get; set; }

        // Training & Development
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<EmployeeCourse> EmployeeCourses { get; set; }

        // Internal Social & Utilities
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetAllocation> AssetAllocations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Relationships

            // Department - Manager (Employee)
            builder
                .Entity<Department>()
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cycle

            // Department - Parent Department
            builder
                .Entity<Department>()
                .HasOne(d => d.ParentDepartment)
                .WithMany(d => d.ChildDepartments)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee - Manager
            builder
                .Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Decimal precision for Money types
            builder
                .Entity<JobTitle>()
                .Property(j => j.BaseSalaryMin)
                .HasColumnType("decimal(18,2)");

            builder
                .Entity<JobTitle>()
                .Property(j => j.BaseSalaryMax)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Contract>().Property(c => c.BaseSalary).HasColumnType("decimal(18,2)");

            // Fix Asset PurchasePrice precision
            builder.Entity<Asset>().Property(a => a.PurchasePrice).HasColumnType("decimal(18,2)");

            // Fix Cycle: Comment -> Author (Employee)
            builder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict); // Important: Stop cascade from Employee -> Comment
        }
    }
}
