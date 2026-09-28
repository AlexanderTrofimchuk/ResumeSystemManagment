using ResumeSystemManagement.Application.DTOs.Statistics;

namespace ResumeSystemManagement.Application.Interfaces.Statistics;

public interface IStatisticsService
{
    Task<StatisticsDto> GetStatisticsAsync();
}