using Domain.ViewModels.Test;

namespace UI.Controllers;

public class TestController : Controller
{
    private readonly ITestControlService _testControlService;
    private static string _testTitle;
     
    public TestController(ITestControlService testControlService)
    {
        _testControlService = testControlService;
    }

    [HttpGet]
    public async Task<IActionResult> ViewList()
    {
        var datas = await _testControlService.GetDataAsync();
        
        return View(datas.Data);
    }

    [HttpPost]
    public async Task<IActionResult> AddNewQuestion(AddNewQuestionViewModel model)
    {
        model.TestTitle = _testTitle;
        var res = await _testControlService.AddNewQuestionAsync(model);
            if(res.Data)
                return View("ContWindow");
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ViewTest(int id)
    {
        var result = await _testControlService.GetTestViewPage(id);
        if(result.StatusCode == HttpStatusCode.BadRequest)
            return View("Error", "404");
        return View(result.Data);
    }

    [HttpGet]
    public IActionResult AddNewQuestion(ParametersOfQuestionViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = new AddNewQuestionViewModel
            {
                Parameters = model
            };
            return View(result);
        }
        return View("ParametersOfQuestion", model);
    }

    [HttpGet]
    public IActionResult AddNewTest()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ContWindow()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Check(ContWindowViewModel model)
    {
        if(ModelState.IsValid)
        {
            if (model.Cont == "Yes")
                return RedirectToAction("ParametersOfQuestion");

             return RedirectToAction("ContAddNewResult");
        }
        return View("ContWindow", model);
    }

    [HttpPost]
    public async Task<IActionResult> CountResultOfTest(ViewTestViewModel model)
    {
        if(ModelState.IsValid)
        {
            model.Email = User.Identity.Name;
            var result = await _testControlService.CountTestResultAsync(model);
            if(result.StatusCode == HttpStatusCode.OK)
                return View("ShowResult", result.Description + "   Count: " + result.Data.ToString());

            return View("Error", result.Description);
        }
        return View();
    }

    [HttpGet]
    public IActionResult ShowResult(int result)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddNewTest(AddNewTestViewModel model)
    {
        if(ModelState.IsValid)
        {
            model.Author = User.Identity.Name;
            var resp = await _testControlService.AddNewTestAsync(model);
            if(resp.StatusCode == HttpStatusCode.OK)
            {
                _testTitle = model.Title;
                return View("ParametersOfQuestion");
            }
            else
            {
                return View("Error", "400");
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult ParametersOfQuestion()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ContWindowResult()
    {
        return View();
    }


    
    [HttpGet]
    public async Task<IActionResult> ContAddNewResult()
    {
        var res = await _testControlService.GetTestForResultPage(_testTitle);

        return View(res.Data);
    }

    [HttpGet]
    public async Task<IActionResult> AddNewResult()
    {
        var res = await _testControlService.GetTestForResultPage(_testTitle);

        return View(res);
    }

    [HttpPost]
    public async Task<IActionResult> AddNewResult(AddNewResultViewModel model)
    {
        if(ModelState.IsValid)
        {
            model.testTitle = _testTitle;
            var response =  await _testControlService.AddNewResultAsync(model);
            if(response.Data)
                return RedirectToAction("ContWindowResult");

            return View("Error", "400");
        }
        return View();
    }

    [HttpPost]
    public IActionResult CheckResult(ContWindowViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Cont == "Yes")
                return RedirectToAction("Co ntAddNewResult");

            return View("Success", "Test was added");
        }
        return View("ContWindowResult", model);
    }
}
 