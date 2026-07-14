using BackEnd.Repositories.Companies;
using BackEnd.Repositories.Departments;
using BackEnd.Repositories.Users;
using BackEnd.Services.Companies;
using BackEnd.Services.Departments;
using BackEnd.Services.Users;

namespace BackEnd.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanyService, CompanyService>();

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDepartmentService, DepartmentService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}