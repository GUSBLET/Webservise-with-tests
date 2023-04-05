using System.Collections.Generic;

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

    public async Task<BaseResponse<bool>> AddNewResultAsync(AddNewResultViewModel model)
    {
        try
        {
            var result = new Result
            {
                Name = model.Result
            };
            await _resultRepository.Add(result);
            result = await _resultRepository.Select().Where(x => x.Name == model.Result).FirstOrDefaultAsync();
            var test = await _testRepository.Select().Where(x => x.Title == model.testTitle).FirstOrDefaultAsync();
            var testResult = new ResultTest
            {
                MinimumOfDiapason = model.PowerOfResult[0],
                MaximumOfDiapason = model.PowerOfResult[1],
                ResultId = result.Id,
                TestId = test.Id
            };
            if (await _resultTestRepository.Add(testResult))
                return new BaseResponse<bool>
                {
                    Data = true
                };

            return new BaseResponse<bool>
            {
                Data = false
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

    public async Task<BaseResponse<int>> CountTestResultAsync(ViewTestViewModel model)
    {
        try
        {
            int count = 0;
            var test = await _testRepository.Select().Where(x => x.Id == model.TestId).Include(x => x.Questions).FirstOrDefaultAsync();
            var questionsWithAnswers = new List<Question>();
            for (int i = 0; i < test.Questions.ToArray().Length; i++)
            {
                var question = _questionRepository.Select().Where(x => x.QuestionId == test.Questions.ToArray()[i].QuestionId).Include(x => x.Answers).FirstOrDefault();
                questionsWithAnswers.Add(question);
            }
            test.Questions = questionsWithAnswers;
            int j = 0;
            foreach (var item in test.Questions)
            {
                var corentTest = test.Questions.Where(x => x.QuestionId == item.QuestionId).FirstOrDefault();
                if (corentTest.Answers.ToArray()[Convert.ToInt32(model.AnswersChoise[j])].PowerOfAnswer == true)
                    count++;
                j++;
            }

            var value = await _userRepository.Select().Where(x => x.Email == model.Email).FirstOrDefaultAsync();

            var testProfile = new ProfileTest
            {
                ProfileId = value.ProfileId,
                TestId = model.TestId,
                TestResult = count
            };
            var buffer = await _profileTestRepository.Select()
                .Where(x => x.TestId == model.TestId && x.ProfileId == value.ProfileId)
                .FirstOrDefaultAsync();
            if ( buffer is null )
                await _profileTestRepository.Add(testProfile);
            else
            {
                buffer.TestResult = count;
                await _profileTestRepository.Update(buffer);
            }

            var resultOfTest = await _resultTestRepository.Select()
                .Where(x => x.MaximumOfDiapason <= count && x.MaximumOfDiapason >= count)
                .FirstOrDefaultAsync();

            if(resultOfTest != null)
            {
                var last = await _resultRepository.Select().Where(x => x.Id == resultOfTest.ResultId).FirstOrDefaultAsync();
                return new BaseResponse<int>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = count,
                    Description = last.Name
                };
            }
            return new BaseResponse<int>
            {
                StatusCode = HttpStatusCode.OK,
                Data = count,
                
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

    public Task<BaseResponse<TestPassingViewModel>> GetTestAsync(int id)
    {
        try
        {
            return null;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<AddNewResultViewModel>> GetTestForResultPage(string title)
    {
        try
        {
            var res = await _testRepository.Select().Where(x => x.Title == title).Include(x => x.Questions).FirstOrDefaultAsync();
            if(res is null)
                return new BaseResponse<AddNewResultViewModel> { StatusCode = HttpStatusCode.BadRequest };
            var test = new AddNewResultViewModel()
            {
                TestQuestion = res.Questions.Count
            };
            return new BaseResponse<AddNewResultViewModel> { StatusCode = HttpStatusCode.OK, Data =  test};
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<ViewTestViewModel>> GetTestViewPage(int id)
    {
        try
        {
            var res = _testRepository.Select().Where(x => x.Id == id)
                .Include(x => x.Questions).FirstOrDefault();
            if(res is null)
                return new BaseResponse<ViewTestViewModel> { StatusCode = HttpStatusCode.BadRequest };
            var questionsWithAnswers = new List<Question>();
            for (int i = 0; i < res.Questions.ToArray().Length; i++)
            {
                var question = _questionRepository.Select().Where(x => x.QuestionId == res.Questions.ToArray()[i].QuestionId).Include(x => x.Answers).FirstOrDefault();
                questionsWithAnswers.Add(question);
            }

            ViewTestViewModel result = new();
            result.TestId = id;
            result.Questions = new List<Question>();
            foreach (var item in res.Questions)
            {
                result.Questions.Add(item);
            }

            return new BaseResponse<ViewTestViewModel> 
            {
                StatusCode = HttpStatusCode.OK ,
                Data = result
            };
        }
        catch (Exception)
        {

            throw;
        }
    }
}
