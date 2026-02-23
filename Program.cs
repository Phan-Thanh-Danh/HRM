using HRM.Data;
using HRM.Models;
using HRM.Services.Admin; // Added here
using HRM.Services.HR; // Added here
using HRM.Services.Learning; // Added here
using HRM.Services.Recruitment; // Added here
using HRM.Services.Salary; // Added here
using HRM.Services.Social; // Added here
using HRM.Services.Time; // Added here
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder
    .Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(HRM.Mappings.AutoMapperProfile));

// Register Domain Services
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IOnboardingService, OnboardingService>();

builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();

builder.Services.AddScoped<IPayrollService, PayrollService>();

builder.Services.AddScoped<IRecruitmentService, RecruitmentService>();

builder.Services.AddScoped<ILearningService, LearningService>();

builder.Services.AddScoped<INewsfeedService, NewsfeedService>();
builder.Services.AddScoped<IAssetService, AssetService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(
    new StaticFileOptions
    {
        ServeUnknownFileTypes = true,
        DefaultContentType = "application/octet-stream",
    }
);

// Original had app.MapStaticAssets(); which is new in .NET 9.
// I will keep MapStaticAssets but usually UseStaticFiles is also needed for library assets.
// Let's stick to standard pattern for now.

app.UseRouting();

app.UseAuthentication(); // Added Authentication
app.UseAuthorization();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Ensure database is created (Code First)
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();

        // Seed Data
        await DbSeeder.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
