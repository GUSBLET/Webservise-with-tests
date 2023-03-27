namespace BusinessLogicLayer.Services;

public class TestControlService : ITestControlService
{
    private readonly IBaseRepository<Test> _testRepository;
    private readonly IBaseRepository<Question> _questionRepository;
    private readonly IBaseRepository<Answer> _answerRepository;
    private readonly IBaseRepository<Result> _resultRepository;
    private readonly IBaseRepository<ResultTest> _resultTestRepository;
    private readonly IBaseRepository<ProfileTest> _profileTestRepository;
}
