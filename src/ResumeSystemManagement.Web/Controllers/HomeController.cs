using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.Interfaces.Statistics;
using ResumeSystemManagement.Web.ViewModels;

namespace ResumeSystemManagement.Web.Controllers;

public class HomeController(IStatisticsService statisticsService) : Controller
{
    private readonly IStatisticsService _statisticsService = statisticsService;

    public async Task<IActionResult> Index()
    {
        var statistics = await _statisticsService.GetStatisticsAsync();
        return View(statistics);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}