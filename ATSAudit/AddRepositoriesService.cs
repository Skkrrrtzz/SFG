using QA_Audit_Fresh.Repositories;

public static class AddRepositoriesService
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuditPlanRepository, AuditPlanRepository>();
        services.AddScoped<IConformityRepository, ConformityRepository>();
        services.AddScoped<ICPARRepository, CPARRepository>();
        return services;
    }
}