using MySampleFW.Helper.Validations.UserValidator;
using MySampleFW.UserDomain.Data.Interfaces;
using MySampleFW.UserDomain.Repositories.Cache;
using MySampleFW.UserDomain.Repositories.CacheInterfaces;
using MySampleFW.UserDomain.Repositories.RepositoryManager;
using MySampleFW.UserDomain.Services.Interfaces;
using MySampleFW.UserDomain.Services.ServicesManager;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;
namespace MySampleFW.Helper.Dependency;
public static class UserDependencyRegister
{
    public static void ConfigureUserDependency(this IServiceCollection services)
    {
        services.AddScoped<IUserInfoRepository, UserInfoRepository>();
        services.AddScoped<IUserInfoCache, UserInfoCache>();
        services.AddScoped<IUserInfoServices, UserInfoServices>();
        services.AddValidatorsFromAssemblyContaining<UserInfoValidator>();

        services.AddScoped<IUsersRolesServices, UsersRolesServices>();
        services.AddScoped<IUsersRolesRepository, UsersRolesRepository>();
        services.AddValidatorsFromAssemblyContaining<UsersRolesValidator>();
    }
}   