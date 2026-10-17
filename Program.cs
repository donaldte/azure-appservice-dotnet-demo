using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var app = builder.Build();

// Sert wwwroot/index.html à la racine du site
app.UseDefaultFiles();
app.UseStaticFiles();

// Endpoint de santé : utilisable par la fonctionnalité "Health check" d'App Service
app.MapHealthChecks("/health");

// Infos sur l'application et son environnement d'exécution
app.MapGet("/api/info", (IConfiguration config, IHostEnvironment env) =>
{
    // Sur Azure App Service, les "App settings" deviennent des variables d'environnement.
    // Un réglage nommé "Greeting" remplace donc la valeur d'appsettings.json.
    var siteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
    var instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");

    return Results.Ok(new
    {
        greeting = config["Greeting"] ?? "Hello!",
        runningOnAppService = siteName is not null,
        siteName = siteName ?? "(exécution locale)",
        instanceId = instanceId is null ? "(n/a)" : instanceId[..Math.Min(12, instanceId.Length)],
        machineName = Environment.MachineName,
        environment = env.EnvironmentName,
        framework = RuntimeInformation.FrameworkDescription,
        os = RuntimeInformation.OSDescription,
        utcTime = DateTime.UtcNow
    });
});

app.Run();
