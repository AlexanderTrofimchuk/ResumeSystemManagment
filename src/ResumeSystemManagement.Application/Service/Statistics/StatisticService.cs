using ResumeSystemManagement.Application.DTOs.Statistic;
using ResumeSystemManagement.Application.Interfaces.Statistics;
using ResumeSystemManagement.Core.Interfaces.Repositories.PositionRepository;
using ResumeSystemManagement.Core.Interfaces.Repositories.UserRepository;

namespace ResumeSystemManagement.Application.Service.Statistics;

public class StatisticService(IPositionRepository positionRepository, IUserRepository userRepository)
    : IStatisticService
{
    private readonly IPositionRepository  _positionRepository = positionRepository;
    private readonly IUserRepository  _userRepository = userRepository;

    public async Task<StaticticeDto> GetStatisticsAsync()
    {
        var countCandidate = await _userRepository.GetCountCandidate();
        StaticticeDto statictice = new(){
            TotalPositions = await _positionRepository.GetCountAsync(),
            TotalCandidates = countCandidate,
        };
        await PopulateMostPopularPositions(statictice);
        await PopulateLatestPosition(statictice);
        
        return statictice;
    }

    private int GetResumeCount(int positionId) => _positionRepository.GetCountResume(positionId);

    private async Task PopulateLatestPosition(StaticticeDto statictice)
    {
        var latestPostion =  await _positionRepository.GetLatestPositionAsync();
        statictice.LatestPositions = latestPostion.Select(p => new LatestPosition
        {
            Id = p.Id,
            Level = p.Level, Title = p.Title,
            Created = p.CreateAt,
        }).ToList();
    }

    private async Task PopulateMostPopularPositions(StaticticeDto statictice)
    {
        var famostPosition = await _positionRepository.GetPopularPositionAsync();
        statictice.MostPopularPositions = famostPosition.Select(p => new PopularPosition
        {
            Id = p.Id,
            SubmittedCvs = GetResumeCount(p.Id),
            Title = p.Title,
        }).ToList();
    }
}