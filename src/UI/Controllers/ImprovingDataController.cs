namespace UI.Controllers;

public class ImprovingDataController : Controller
{
    private readonly IImprovingDataService _improvingDataService;

    public ImprovingDataController(IImprovingDataService improvingDataService)
    {
        _improvingDataService = improvingDataService;
    }

    [HttpGet]
    public IActionResult AddNewImprovingData()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddNewImprovingData(AddNewImprovingDataViewModel model)
    {
        if(ModelState.IsValid)
        {
            model.Email = User.Identity.Name ?? "";
            var result = await _improvingDataService.AddNewItemAsync(model); 

            if(result == HttpStatusCode.OK)
                return View("Success", "Improving data have been added");

            return View("Error", "Improving data have not been added, server error");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ViewList()
    {
        var datas = await _improvingDataService.GetDataAsync();
        return View(datas.Data);
    }

    [HttpGet]
    public async Task<IActionResult> ViewImprovingData(int id)
    {
        if(ModelState.IsValid)
        {
            var readMark = await _improvingDataService.SetToReadDataAsync(User.Identity.Name ?? "", id);
            if(readMark == HttpStatusCode.OK)
            {
                var result = _improvingDataService.GetDataByIdAsync(id);

                return View(result.Result.Data);
            }
        }
        return View("Error", "Error 404 post did not find");
    }

}
