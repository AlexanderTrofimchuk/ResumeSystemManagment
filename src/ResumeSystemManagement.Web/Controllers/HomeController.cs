using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResumeSystemManagement.Application.Interfaces.Statistics;
using ResumeSystemManagement.Web.ViewModels;

namespace ResumeSystemManagement.Web.Controllers;

public class HomeController(IStatisticService statisticService) : Controller
{
    private readonly IStatisticService _statisticService = statisticService;

    public async Task<IActionResult> Index()
    {
        var statistics = await _statisticService.GetStatisticsAsync();
        return View(statistics);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}