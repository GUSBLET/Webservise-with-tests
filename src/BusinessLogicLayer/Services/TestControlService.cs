using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services;

public class TestControlService : ITestControlService
{
    private readonly IBaseRepository<Test> _testRepository;
    private readonly IBaseRepository<Question> _questionRepository;
    private readonly IBaseRepository<Answer> _answerRepository;
    private readonly IBaseRepository<Result> _resultRepository;
    private readonly IBaseRepository<ResultTest> _resultTestRepository;
    private readonly IBaseRepository<ProfileTest> _profileTestRepository;
    private readonly IBaseRepository<User> _userRepository;

    public TestControlService(
        IBaseRepository<Test> testRepository, 
        IBaseRepository<Question> questionRepository, 
        IBaseRepository<Answer> answerRepository, 
        IBaseRepository<Result> resultRepository, 
        IBaseRepository<ResultTest> resultTestRepository, 
        IBaseRepository<ProfileTest> profileTestRepository,
        IBaseRepository<User> userRepository)
    {
        _testRepository = testRepository;
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
        _resultRepository = resultRepository;
        _resultTestRepository = resultTestRepository;
        _profileTestRepository = profileTestRepository;
        _userRepository = userRepository;
    }

    public async Task<BaseResponse<List<ViewListTestViewModel>>> GetDataAsync()
    {
        try
        {
            var result = await (from impr in _testRepository.Select()
                                join c in _userRepository.Select() on impr.Author.Id equals c.Id
                                select new ViewListTestViewModel
                                {
                                    TimePassing = impr.TimePassing,
                                    Author = impr.Author,
                                    Description = impr.Description,
                                    Name = impr.Title,
                                    Id = impr.Id
                                }).ToListAsync();
            if (result.Count > 0)
                return new BaseResponse<List<ViewListTestViewModel>>
                {
                    Data = result,
                    StatusCode = HttpStatusCode.OK
                };

            return new BaseResponse<List<ViewListTestViewModel>>
            {
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        catch (Exception)
        {

            throw;
        }
    }
}
