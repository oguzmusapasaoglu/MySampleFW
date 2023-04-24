using MyCore.Cache.Interfaces;
using MyCore.Dapper.Interfaces;
using MyCore.Dapper.Factory;
using MyCore.LogManager.Services;
using MyCore.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using MyCore.Elastic.Interfaces;
using MyCore.Elastic.Services;
using MySampleFW.Helper.Validations.Interfaces;
using MySampleFW.Helper.Validations.Helper;
using MyCore.Common.Interfaces;
using MyCore.Common.Helper;

namespace MySampleFW.Helper.Dependency;

public static class CoreDependencyRegister
{
    public static void ConfigureCoreDependency(this IServiceCollection services)
    {
        services.AddScoped<IMemCacheServices, MemCacheServices>();
        services.AddScoped<IConnectionFactory, ConnectionFactory>();
        services.AddScoped<IDbFactory, DbFactory>();
        services.AddScoped<ILogServices, LogServices>();
        services.AddScoped<IElasticManageServices, ElasticManageServices>();
        services.AddScoped<IValidateManager, ValidateManager>();
        services.AddScoped<IFileUploadHelper, FileUploadHelper>();
    }
}
