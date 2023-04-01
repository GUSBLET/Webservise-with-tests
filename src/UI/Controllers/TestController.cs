using Domain.Entities;
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

    [HttpPost]
    public IActionResult Check(ContWindowViewModel model)
    {
        if(ModelState.IsValid)
        {
            if (model.Cont == "Yes")
                return RedirectToAction("ParametersOfQuestion");
        }
        return View("ContWindow", model);
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

}
