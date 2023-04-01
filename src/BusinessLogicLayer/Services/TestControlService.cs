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

    public async Task<BaseResponse<bool>> AddNewQuestionAsync(AddNewQuestionViewModel model)
    {
        try
        {
            var r = await _testRepository.Select().Where(x => x.Title == model.TestTitle).FirstOrDefaultAsync();
            
            var question = new Question()
            {
                QuestionText = model.QuestionText,
                Test = r
            };
            var test = await _testRepository.Select().Where(x => x.Title == model.TestTitle).FirstOrDefaultAsync();
            if (test.Questions is null)
                test.Questions = new List<Question>();
            if (!test.Questions.Contains(question))
                test.Questions.Add(question);
            if (!await _questionRepository.Add(question))
                return new BaseResponse<bool>
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
            
            await _testRepository.Update(test);
            model.AnswersPower = new List<bool>();
            for (int i = 0; i < model.Answers.Count; i++)
            {
                if (model.AnswersPowerHtml == i.ToString())
                    model.AnswersPower.Add(true);
                model.AnswersPower.Add(false);
            }
            for (int i = 0;i  < model.Answers.Count(); i++)
            {
                var resultAnswer = await _answerRepository.Select()
                    .Where(x => x.Name == model.Answers[i] && x.PowerOfAnswer == model.AnswersPower[i]).FirstOrDefaultAsync();
                if(resultAnswer is null)
                {
                    var newAnswer = new Answer()
                    {
                        PowerOfAnswer = model.AnswersPower[i],
                        Name = model.Answers[i],
                    };
                    await _answerRepository.Add(newAnswer);
                    resultAnswer = newAnswer;
                }
                if(question.Answers is null)
                    question.Answers = new List<Answer>();
                if (!question.Answers.Contains(resultAnswer))
                    question.Answers.Add(resultAnswer);                    
            }
            await _questionRepository.Update(question);

            return new BaseResponse<bool>
            {
                Data = true
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<bool>> AddNewTestAsync(AddNewTestViewModel model)
    {
        try
        {
            var test = await _testRepository.Select().Where(x => x.Title == model.Title).FirstOrDefaultAsync();
            if (test != null)
                return new BaseResponse<bool>
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
            var r = await _userRepository.Select().Where(x => x.Email == model.Author).FirstOrDefaultAsync();
            test = new Test
            {
                Title = model.Title,
                Description = model.Description,
                TimePassing = model.TimePassing,
                Author = r,
            };

            if (await _testRepository.Add(test))
                return new BaseResponse<bool>
                {
                    StatusCode = HttpStatusCode.OK
                };

            return new BaseResponse<bool>
            {
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        catch (Exception)
        {

            throw;
        }
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
