using dotenv.net;
using ResumeSystemManagement.Application;
using ResumeSystemManagement.Infrastructure;

DotEnv.Load(
    new DotEnvOptions(envFilePaths:[Path.GetFullPath(Path.Combine("..","..",".env"))]));

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplication();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await services.InitializeDbAndRoles();
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await app.RunAsync();