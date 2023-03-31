namespace UI.Controllers;

public class TestController : Controller
{
    private readonly ITestControlService _testControlService;

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

    [HttpGet]
    public IActionResult AddNewTest()
    {
        return View();
    }

}
