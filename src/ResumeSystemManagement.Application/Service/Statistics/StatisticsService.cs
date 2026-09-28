using ResumeSystemManagement.Application.DTOs.Positions;
using ResumeSystemManagement.Application.DTOs.Statistics;
using ResumeSystemManagement.Application.Interfaces.Statistics;
using ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;

namespace ResumeSystemManagement.Application.Service.Statistics;

public class StatisticsService(IPositionRepository positionRepository, IUserRepository userRepository)
    : IStatisticsService
{
    private readonly IPositionRepository  _positionRepository = positionRepository;
    private readonly IUserRepository  _userRepository = userRepository;

    public async Task<StatisticsDto> GetStatisticsAsync()
    {
        var countCandidate = await _userRepository.GetCountCandidate();
        StatisticsDto statistics = new(){
            TotalPositions = await _positionRepository.GetPublishCountAsync(),
            TotalCandidates = countCandidate,
        };
        await PopulateMostPopularPositions(statistics);
        await PopulateLatestPosition(statistics);
        
        return statistics;
    }

    private int GetResumeCount(int positionId) => _positionRepository.GetCountResume(positionId);

    private async Task PopulateLatestPosition(StatisticsDto statistics)
    {
        var latestPostion =  await _positionRepository.GetLatestPositionAsync();
        statistics.LatestPositions = latestPostion.Select(p => new LatestPosition
        {
            Id = p.Id,
            Level = p.Level, Title = p.Title,
            Created = p.CreateAt,
        }).ToList();
    }

    private async Task PopulateMostPopularPositions(StatisticsDto statistics)
    {
        var famostPosition = await _positionRepository.GetPopularPositionAsync();
        statistics.MostPopularPositions = famostPosition.Select(p => new PopularPosition
        {
            Id = p.Id,
            SubmittedResumes = GetResumeCount(p.Id),
            Title = p.Title,
        }).ToList();
    }
}