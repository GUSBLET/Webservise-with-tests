namespace UI;

public static class Initializer
{
    public static void InitializeRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<Answer>, AnswerRepository>();
        services.AddScoped<IBaseRepository<User>, UserRepository>();
        services.AddScoped<IBaseRepository<Profile>, ProfileRepository>();
        services.AddScoped<IBaseRepository<ImprovingData>, ImprovingDataRepository>();
        services.AddScoped<IBaseRepository<ProfileTest>, ProfileTestRepository>();
        services.AddScoped<IBaseRepository<Question>, QuestionRepository>();
        services.AddScoped<IBaseRepository<Result>, ResultRepository>();
        services.AddScoped<IBaseRepository<ResultTest>, ResultTestRepository>();
        services.AddScoped<IBaseRepository<Test>, TestRepository>();
        services.AddScoped<IBaseRepository<ProfileImprovingData>, ProfileImprovingDataRepository>();
    }

    public static void InitializeServices(this IServiceCollection services)
    {
        services.AddSingleton<IMailService, MailService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IImprovingDataService, ImprovingDataService>();
        services.AddScoped<ITestControlService, TestControlService>();
    }
}