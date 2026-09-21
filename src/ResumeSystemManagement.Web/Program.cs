using dotenv.net;
using Microsoft.AspNetCore.HttpOverrides;
using ResumeSystemManagement.Application;
using ResumeSystemManagement.Infrastructure;

DotEnv.Load(
    new DotEnvOptions(envFilePaths:[Path.GetFullPath(Path.Combine("..","..",".env"))]));

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplication();
builder.Services.AddControllersWithViews();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

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