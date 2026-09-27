using System.Globalization;
using dotenv.net;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using ResumeSystemManagement.Application;
using ResumeSystemManagement.Infrastructure;

DotEnv.Load(
    new DotEnvOptions(envFilePaths:[Path.GetFullPath(Path.Combine("..","..",".env"))]));

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplication();
builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

builder.Services.AddLocalization(option =>
{
    option.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(option =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("de-DE")
    };
    option.DefaultRequestCulture = new RequestCulture("en-US");
    option.SupportedUICultures = supportedCultures;
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();
app.UseRequestLocalization();
app.UseForwardedHeaders();

app.UseExceptionHandler("/Home/Error");

await app.Services.InitializeDatabase();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await app.RunAsync();