using QA_Audit_Fresh.Repositories;

public static class AddRepositoriesService
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuditPlansRepository, AuditPlansService>();
        services.AddScoped<IConformitiesRepository, ConformitiesService>();
        services.AddScoped<ICPARsRepository, CPARsService>();
        services.AddScoped<ICorrectionsRepository, CorrectionsService>();
        services.AddScoped<ICorrectiveActionsRepository, CorrectiveActionsService>();
        services.AddScoped<IPreventiveActionsRepository, PreventiveActionsService>();
        services.AddScoped<IUsersRepository, UsersService>();
        return services;
    }
}