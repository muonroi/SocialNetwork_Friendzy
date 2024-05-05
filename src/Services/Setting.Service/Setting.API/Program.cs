Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SerilogAction.Configure);
Log.Information($"Starting {builder.Environment.ApplicationName} API up");
try
{
    // Add services to the container.

    _ = builder.Services.AddControllers();
    _ = builder.Services.AddInfrastructureServices(builder.Configuration);
    _ = builder.Services.AddApplicationServices();
    builder.AddAppConfigurations();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    _ = builder.Services.AddEndpointsApiExplorer();
    _ = builder.Services.AddSwaggerGen();

    WebApplication app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        _ = app.UseSwagger();
        _ = app.UseSwaggerUI();
    }
    using (IServiceScope scope = app.Services.CreateScope())
    {
        SettingDbContextSeed services = scope.ServiceProvider.GetRequiredService<SettingDbContextSeed>();
        await services.InitialiseAsync();
        await services.SeedAsync();
    }

    // app.UseHttpsRedirection();

    _ = app.UseAuthorization();

    _ = app.MapControllers();
    _ = app.UseCors();
    _ = app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    _ = app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information("Shut down Setting Service complete");
    Log.CloseAndFlush();
}