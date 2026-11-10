using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        ".logs/start-host-log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting the application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
    builder.Services.AddSerilog((_, configuration) =>
        configuration.ReadFrom.Configuration(builder.Configuration));

    builder.Services.AddRazorPages();

    var app = builder.Build();
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSerilogRequestLogging();

    app.UseRouting();
    app.UseAuthorization();

    app.MapRazorPages();
    app.Run();
}

catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}

finally
{
    Log.CloseAndFlush();
}