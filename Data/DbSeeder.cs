using HRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRM.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            // 1. Roles
            string[] roleNames =
            {
                "SuperAdmin",
                "HRManager",
                "CBSpecialist",
                "TalentAcquisition",
                "DepartmentManager",
                "Employee",
            };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. SuperAdmin
            if (await userManager.FindByEmailAsync("admin@hrm.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@hrm.com",
                    Email = "admin@hrm.com",
                    FullName = "Super Administrator",
                    EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "SuperAdmin");
            }

            // 3. Departments
            if (!context.Departments.Any())
            {
                var depts = new List<Department>
                {
                    new Department
                    {
                        Name = "Human Resources",
                        Description = "Manages employee lifecycle",
                    },
                    new Department
                    {
                        Name = "IT Department",
                        Description = "Technology and Infrastructure",
                    },
                    new Department { Name = "Sales", Description = "Revenue generation" },
                    new Department { Name = "Marketing", Description = "Brand and Promotion" },
                    new Department
                    {
                        Name = "Finance",
                        Description = "Accounting and Payroll base",
                    },
                };
                context.Departments.AddRange(depts);
                await context.SaveChangesAsync();
            }

            // 4. Job Titles
            if (!context.JobTitles.Any())
            {
                var titles = new List<JobTitle>
                {
                    new JobTitle
                    {
                        Title = "HR Manager",
                        BaseSalaryMin = 20000000,
                        BaseSalaryMax = 40000000,
                    },
                    new JobTitle
                    {
                        Title = "Senior Developer",
                        BaseSalaryMin = 25000000,
                        BaseSalaryMax = 50000000,
                    },
                    new JobTitle
                    {
                        Title = "Sales Executive",
                        BaseSalaryMin = 10000000,
                        BaseSalaryMax = 30000000,
                    },
                    new JobTitle
                    {
                        Title = "Marketing Specialist",
                        BaseSalaryMin = 12000000,
                        BaseSalaryMax = 25000000,
                    },
                    new JobTitle
                    {
                        Title = "Accountant",
                        BaseSalaryMin = 15000000,
                        BaseSalaryMax = 25000000,
                    },
                };
                context.JobTitles.AddRange(titles);
                await context.SaveChangesAsync();
            }

            // 5. Employees & Users
            if (!context.Employees.Any())
            {
                var depts = await context.Departments.ToListAsync();
                var titles = await context.JobTitles.ToListAsync();

                var employees = new List<Employee>
                {
                    new Employee
                    {
                        FullName = "Nguyen Van A",
                        Email = "a.nguyen@hrm.com",
                        DepartmentId = depts[0].Id,
                        JobTitleId = titles[0].Id,
                        Status = EmployeeStatus.Active,
                        JoinDate = DateTime.Today.AddYears(-2),
                    },
                    new Employee
                    {
                        FullName = "Tran Thi B",
                        Email = "b.tran@hrm.com",
                        DepartmentId = depts[1].Id,
                        JobTitleId = titles[1].Id,
                        Status = EmployeeStatus.Active,
                        JoinDate = DateTime.Today.AddYears(-1),
                    },
                    new Employee
                    {
                        FullName = "Le Van C",
                        Email = "c.le@hrm.com",
                        DepartmentId = depts[2].Id,
                        JobTitleId = titles[2].Id,
                        Status = EmployeeStatus.Active,
                        JoinDate = DateTime.Today.AddMonths(-6),
                    },
                    new Employee
                    {
                        FullName = "Pham Thi D",
                        Email = "d.pham@hrm.com",
                        DepartmentId = depts[3].Id,
                        JobTitleId = titles[3].Id,
                        Status = EmployeeStatus.Active,
                        JoinDate = DateTime.Today.AddMonths(-3),
                    },
                    new Employee
                    {
                        FullName = "Hoang Van E",
                        Email = "e.hoang@hrm.com",
                        DepartmentId = depts[4].Id,
                        JobTitleId = titles[4].Id,
                        Status = EmployeeStatus.Probation,
                        JoinDate = DateTime.Today.AddDays(-15),
                    },
                };
                context.Employees.AddRange(employees);
                await context.SaveChangesAsync();

                // Create Users for them
                foreach (var emp in employees)
                {
                    if (!string.IsNullOrEmpty(emp.Email) && await userManager.FindByEmailAsync(emp.Email) == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = emp.Email,
                            Email = emp.Email,
                            FullName = emp.FullName,
                            EmailConfirmed = true,
                            EmployeeId = emp.Id,
                        };
                        await userManager.CreateAsync(user, "User@123");
                        await userManager.AddToRoleAsync(user, "Employee");
                        if (emp.DepartmentId == depts[0].Id)
                            await userManager.AddToRoleAsync(user, "HRManager");
                    }
                }
            }

            // Re-fetch employees
            var allEmps = await context.Employees.ToListAsync();

            // 6. Contracts
            if (!context.Contracts.Any())
            {
                foreach (var emp in allEmps)
                {
                    context.Contracts.Add(
                        new Contract
                        {
                            EmployeeId = emp.Id,
                            ContractType = ContractType.IndefiniteTerm, // Fixed
                            StartDate = emp.JoinDate,
                            BaseSalary = 15000000 + (emp.Id * 1000000), // Fixed property name
                            Status = ContractStatus.Active,
                            ContractNumber = $"HD-{emp.Id:000}" // Added required field
                        }
                    );
                }
                await context.SaveChangesAsync();
            }

            // 7. Shifts
            if (!context.Shifts.Any())
            {
                context.Shifts.AddRange(
                    new Shift
                    {
                        Name = "Morning Shift",
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(12, 0, 0),
                    },
                    new Shift
                    {
                        Name = "Afternoon Shift",
                        StartTime = new TimeSpan(13, 0, 0),
                        EndTime = new TimeSpan(17, 0, 0),
                    },
                    new Shift
                    {
                        Name = "Office Hours",
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(17, 0, 0),
                    },
                    new Shift
                    {
                        Name = "Night Shift",
                        StartTime = new TimeSpan(22, 0, 0),
                        EndTime = new TimeSpan(6, 0, 0),
                        IsOvernight = true,
                    },
                    new Shift
                    {
                        Name = "Weekend Shift",
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(15, 0, 0),
                    }
                );
                await context.SaveChangesAsync();
            }

            // 8. Attendance
            if (!context.AttendanceRecords.Any())
            {
                var rng = new Random();
                foreach (var emp in allEmps)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var date = DateTime.Today.AddDays(-i);
                        context.AttendanceRecords.Add(
                            new AttendanceRecord
                            {
                                EmployeeId = emp.Id,
                                Date = date,
                                CheckInTime = date.AddHours(8).AddMinutes(rng.Next(0, 30)),
                                CheckOutTime = date.AddHours(17).AddMinutes(rng.Next(0, 30)),
                                Status = AttendanceStatus.Present, // Fixed enum
                            }
                        );
                    }
                }
                await context.SaveChangesAsync();
            }

            // 9. Leave Requests
            if (!context.LeaveRequests.Any())
            {
                context.LeaveRequests.AddRange(
                    new LeaveRequest
                    {
                        EmployeeId = allEmps[0].Id,
                        Type = LeaveType.AnnualLeave,
                        FromDate = DateTime.Today.AddDays(10),
                        ToDate = DateTime.Today.AddDays(11),
                        Reason = "Family trip",
                        Status = LeaveStatus.Pending,
                    },
                    new LeaveRequest
                    {
                        EmployeeId = allEmps[1].Id,
                        Type = LeaveType.SickLeave,
                        FromDate = DateTime.Today.AddDays(-5),
                        ToDate = DateTime.Today.AddDays(-4),
                        Reason = "Flu",
                        Status = LeaveStatus.Approved,
                    },
                    new LeaveRequest
                    {
                        EmployeeId = allEmps[2].Id,
                        Type = LeaveType.UnpaidLeave,
                        FromDate = DateTime.Today.AddDays(20),
                        ToDate = DateTime.Today.AddDays(20),
                        Reason = "Personal matter",
                        Status = LeaveStatus.Pending,
                    },
                    new LeaveRequest
                    {
                        EmployeeId = allEmps[3].Id,
                        Type = LeaveType.AnnualLeave,
                        FromDate = DateTime.Today.AddDays(5),
                        ToDate = DateTime.Today.AddDays(7),
                        Reason = "Vacation",
                        Status = LeaveStatus.Approved,
                    },
                    new LeaveRequest
                    {
                        EmployeeId = allEmps[4].Id,
                        Type = LeaveType.MaternityLeave,
                        FromDate = DateTime.Today.AddMonths(1),
                        ToDate = DateTime.Today.AddMonths(7),
                        Reason = "Maternity Leave",
                        Status = LeaveStatus.Pending,
                    }
                );
                await context.SaveChangesAsync();
            }

            // 10. Job Postings
            if (!context.JobPostings.Any())
            {
                var depts = await context.Departments.ToListAsync();
                context.JobPostings.AddRange(
                    new JobPosting
                    {
                        Title = "Senior .NET Developer",
                        DepartmentId = depts[1].Id,
                        Quantity = 2,
                        Deadline = DateTime.Today.AddDays(30),
                        MinSalary = 30000000,
                        MaxSalary = 50000000,
                        Status = JobStatus.Published,
                        Location = "Hanoi",
                    },
                    new JobPosting
                    {
                        Title = "HR Intern",
                        DepartmentId = depts[0].Id,
                        Quantity = 1,
                        Deadline = DateTime.Today.AddDays(15),
                        MinSalary = 3000000,
                        MaxSalary = 5000000,
                        Status = JobStatus.Published,
                        Location = "Hanoi",
                    },
                    new JobPosting
                    {
                        Title = "Sales Manager",
                        DepartmentId = depts[2].Id,
                        Quantity = 1,
                        Deadline = DateTime.Today.AddDays(45),
                        MinSalary = 25000000,
                        MaxSalary = 40000000,
                        Status = JobStatus.Draft,
                        Location = "HCMC",
                    },
                    new JobPosting
                    {
                        Title = "Marketing Executive",
                        DepartmentId = depts[3].Id,
                        Quantity = 3,
                        Deadline = DateTime.Today.AddDays(20),
                        MinSalary = 10000000,
                        MaxSalary = 18000000,
                        Status = JobStatus.Published,
                        Location = "Danang",
                    },
                    new JobPosting
                    {
                        Title = "Accountant",
                        DepartmentId = depts[4].Id,
                        Quantity = 1,
                        Deadline = DateTime.Today.AddDays(10),
                        MinSalary = 12000000,
                        MaxSalary = 20000000,
                        Status = JobStatus.Closed,
                        Location = "Hanoi",
                    }
                );
                await context.SaveChangesAsync();
            }

            // 11. Candidates & Applications
            if (!context.Candidates.Any())
            {
                var job = await context.JobPostings.FirstOrDefaultAsync();
                if (job != null)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        var candidate = new Candidate
                        {
                            FullName = $"Candidate {i}",
                            Email = $"cand{i}@test.com",
                            PhoneNumber = "0987654321",
                        };
                        context.Candidates.Add(candidate);
                        await context.SaveChangesAsync();

                        context.Applications.Add(
                            new Application
                            {
                                CandidateId = candidate.Id,
                                JobPostingId = job.Id,
                                AppliedDate = DateTime.UtcNow,
                                Status = (ApplicationStatus)(i % 5), // Distribute statuses
                            }
                        );
                    }
                    await context.SaveChangesAsync();
                }
            }

            // 12. Courses
            if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    new Course
                    {
                        Title = "Company Onboarding",
                        Description = "Welcome to HRM Corp",
                        IsPublished = true,
                    },
                    new Course
                    {
                        Title = "Cyber Security Basics",
                        Description = "Stay safe online",
                        IsPublished = true,
                    },
                    new Course
                    {
                        Title = "Effective Communication",
                        Description = "Soft skills training",
                        IsPublished = true,
                    },
                    new Course
                    {
                        Title = "Sales Mastery",
                        Description = "For Sales team",
                        IsPublished = true,
                    },
                    new Course
                    {
                        Title = "Advanced Excel",
                        Description = "For Finance team",
                        IsPublished = true,
                    }
                );
                await context.SaveChangesAsync();
            }

            // 13. Posts
            if (!context.Posts.Any())
            {
                context.Posts.AddRange(
                    new Post
                    {
                        Content = "Welcome everyone to our new HRM system!",
                        AuthorId = allEmps[0].Id,
                        CreatedAt = DateTime.UtcNow.AddDays(-5),
                    },
                    new Post
                    {
                        Content = "Don't forget the town hall meeting tomorrow.",
                        AuthorId = allEmps[1].Id,
                        CreatedAt = DateTime.UtcNow.AddDays(-3),
                    },
                    new Post
                    {
                        Content = "Happy Birthday to our CEO!",
                        AuthorId = allEmps[2].Id,
                        CreatedAt = DateTime.UtcNow.AddDays(-2),
                    },
                    new Post
                    {
                        Content = "New policy regarding remote work available on the portal.",
                        AuthorId = allEmps[0].Id,
                        CreatedAt = DateTime.UtcNow.AddDays(-1),
                    },
                    new Post
                    {
                        Content = "Check out the new lunch menu!",
                        AuthorId = allEmps[3].Id,
                        CreatedAt = DateTime.UtcNow.AddHours(-4),
                    }
                );
                await context.SaveChangesAsync();
            }

            // 14. Assets
            if (!context.Assets.Any())
            {
                context.Assets.AddRange(
                    new Asset
                    {
                        Name = "Laptop Dell XPS 15",
                        Code = "IT-001",
                        PurchasePrice = 45000000,
                        PurchaseDate = DateTime.Today.AddMonths(-12),
                        Status = AssetStatus.InUse,
                        CurrentHolderId = allEmps[1].Id,
                    },
                    new Asset
                    {
                        Name = "MacBook Pro M1",
                        Code = "IT-002",
                        PurchasePrice = 50000000,
                        PurchaseDate = DateTime.Today.AddMonths(-6),
                        Status = AssetStatus.Available,
                    },
                    new Asset
                    {
                        Name = "Monitor Dell Ultrasharp",
                        Code = "IT-003",
                        PurchasePrice = 10000000,
                        PurchaseDate = DateTime.Today.AddMonths(-10),
                        Status = AssetStatus.InUse,
                        CurrentHolderId = allEmps[1].Id,
                    },
                    new Asset
                    {
                        Name = "Office Chair Herman Miller",
                        Code = "FUR-001",
                        PurchasePrice = 25000000,
                        PurchaseDate = DateTime.Today.AddYears(-2),
                        Status = AssetStatus.Available,
                    },
                    new Asset
                    {
                        Name = "Projector Sony",
                        Code = "EQ-001",
                        PurchasePrice = 15000000,
                        PurchaseDate = DateTime.Today.AddMonths(-24),
                        Status = AssetStatus.Maintenance,
                    }
                );
                await context.SaveChangesAsync();
            }

            // 15. Payroll Periods (Mock)
            if (!context.PayrollPeriods.Any())
            {
                context.PayrollPeriods.Add(
                    new PayrollPeriod
                    {
                        Name = "Payroll Oct 2026",
                        Month = 10,
                        Year = 2026,
                        FromDate = new DateTime(2026, 10, 1),
                        ToDate = new DateTime(2026, 10, 31),
                        Status = PayrollStatus.Open,
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
