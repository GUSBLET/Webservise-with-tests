namespace BusinessLogicLayer.Services;

public class ImprovingDataService : IImprovingDataService
{
    private readonly IBaseRepository<ImprovingData> _improvingDataRepository;
    private readonly IBaseRepository<Question> _questionRepository;
    private readonly IBaseRepository<Answer> _answerRepository;
    private readonly IBaseRepository<Result> _resultRepository;
    private readonly IBaseRepository<ResultTest> _resultTestRepository;
    private readonly IBaseRepository<ProfileTest> _profileTestRepository;
}
