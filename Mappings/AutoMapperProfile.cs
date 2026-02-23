using AutoMapper;
using HRM.Models;
using HRM.ViewModels.HR;
using HRM.ViewModels.Time;
using HRM.ViewModels.Salary;
using HRM.ViewModels.Recruitment;
using HRM.ViewModels.Learning;
using HRM.ViewModels.Social;
using HRM.ViewModels.Admin;

namespace HRM.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Department Mappings
            CreateMap<Department, DepartmentVM>()
                .ForMember(
                    dest => dest.ParentName,
                    opt =>
                        opt.MapFrom(src =>
                            src.ParentDepartment != null ? src.ParentDepartment.Name : null
                        )
                )
                .ForMember(
                    dest => dest.ManagerName,
                    opt => opt.MapFrom(src => src.Manager != null ? src.Manager.FullName : null)
                )
                .ForMember(
                    dest => dest.EmployeeCount,
                    opt => opt.MapFrom(src => src.Employees.Count)
                );

            CreateMap<DepartmentVM, Department>();

            // Employee Mappings
            CreateMap<Employee, EmployeeVM>();
            CreateMap<EmployeeVM, Employee>()
                .ForMember(dest => dest.AvatarUrl, opt => opt.Ignore()); // Handle file upload separately

            // Contract Mappings
            CreateMap<Contract, ContractVM>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null));
            CreateMap<ContractVM, Contract>();

            // Onboarding Mappings
            CreateMap<OnboardingTask, OnboardingTaskVM>().ReverseMap();

            // Time & Attendance Mappings
            CreateMap<Shift, ShiftVM>().ReverseMap();
            
            CreateMap<AttendanceRecord, AttendanceLogVM>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null));

            CreateMap<LeaveRequest, LeaveRequestVM>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
                .ForMember(dest => dest.ApproverName, opt => opt.MapFrom(src => src.Approver != null ? src.Approver.FullName : null));
            CreateMap<LeaveRequestVM, LeaveRequest>();

            // Payroll Mappings
            CreateMap<SalaryFormula, SalaryFormulaVM>()
                .ForMember(dest => dest.SalaryComponentName, opt => opt.MapFrom(src => src.SalaryComponent != null ? src.SalaryComponent.Name : null))
                .ForMember(dest => dest.IsFixed, opt => opt.MapFrom(src => src.SalaryComponent != null ? src.SalaryComponent.IsFixed : false));
            CreateMap<SalaryFormulaVM, SalaryFormula>();

            CreateMap<PayrollPeriod, PayrollPeriodVM>().ReverseMap();
            
            CreateMap<PayrollRecord, PayrollRecordVM>()
                 .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null));

            // Recruitment Mappings
            CreateMap<JobPosting, JobPostingVM>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null));
            CreateMap<JobPostingVM, JobPosting>();

            CreateMap<Candidate, CandidateVM>();
            CreateMap<CandidateVM, Candidate>()
                .ForMember(dest => dest.CVFilePath, opt => opt.Ignore()); // File upload

            CreateMap<Application, ApplicationCardVM>()
                .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.FullName : null))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobPosting != null ? src.JobPosting.Title : null));

            // Learning Mappings
            CreateMap<Course, CourseVM>();
            CreateMap<CourseVM, Course>();
            CreateMap<Lesson, LessonVM>();
            CreateMap<LessonVM, Lesson>();

            // Social & Utilities Mappings
            CreateMap<Post, PostVM>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.FullName : "Unknown"));
            CreateMap<PostVM, Post>();

            CreateMap<Comment, CommentVM>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.FullName : "Unknown"));
            CreateMap<CommentVM, Comment>();

            CreateMap<Asset, AssetVM>()
                .ForMember(dest => dest.CurrentHolderName, opt => opt.MapFrom(src => (src.CurrentHolderId.HasValue) ? "Assigned" : "Available")); // Simplified
             CreateMap<AssetVM, Asset>();
        }
    }
}
