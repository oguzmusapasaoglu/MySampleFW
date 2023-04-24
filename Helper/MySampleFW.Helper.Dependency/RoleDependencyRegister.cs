using MySampleFW.RoleDomain.Services.Interfaces;
using MySampleFW.RoleDomain.Repositores.Interfaces;
using MySampleFW.RoleDomain.Services.CacheInterfaces;
using MySampleFW.RoleDomain.Services.Cache;
using MySampleFW.RoleDomain.Services.ServicesManager;
using MySampleFW.RoleDomain.Repositores.RepositoryManager;
using Microsoft.Extensions.DependencyInjection;
using MySampleFW.Helper.Validations.RoleValidator;
using FluentValidation;

namespace MySampleFW.Helper.Dependency;
public static class RoleDependencyRegister
{
    public static void ConfigureRoleDependency(this IServiceCollection services)
    {
        services.AddScoped<IRolesServices, RolesServices>();
        services.AddScoped<IRolesCache, RolesCache>();
        services.AddScoped<IRolesRepository, RolesRepository>();
        services.AddValidatorsFromAssemblyContaining<RolesValidator>();

        services.AddScoped<IRolePageObjectServices, RolePageObjectServices>();
        services.AddScoped<IRolePageObjectCache, RolePageObjectCache>();
        services.AddScoped<IRolePageObjectRepository, RolePageObjectRepository>();
        services.AddValidatorsFromAssemblyContaining<RolePageObjectValidator>();

        services.AddScoped<IPagesServices, PagesServices>();
        services.AddScoped<IPagesCache, PagesCache>();
        services.AddScoped<IPagesRepository, PagesRepository>();
        services.AddValidatorsFromAssemblyContaining<PageValidator>();

        services.AddScoped<IPageObjectServices, PageObjectServices>();
        services.AddScoped<IPageObjectCache, PageObjectCache>();
        services.AddScoped<IPageObjectRepository, PageObjectRepository>();
        services.AddValidatorsFromAssemblyContaining<PageObjectValidator>();

        services.AddScoped<IAuthorizationControlCache, AuthorizationControlCache>();
        services.AddScoped<IAuthorizationControlServices, AuthorizationControlServices>();

    }
}
