using ResumeSystemManagement.Application.DTOs.Statistic;

namespace ResumeSystemManagement.Application.Interfaces.Statistics;

public interface IStatisticService
{
    Task<StaticticeDto> GetStatisticsAsync();
}