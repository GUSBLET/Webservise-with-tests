namespace BusinessLogicLayer.Interfaces;

public interface ITestControlService
{
    public Task<BaseResponse<AddNewResultViewModel>> GetTestForResultPage(string title);
    public Task<BaseResponse<ViewTestViewModel>> GetTestViewPage(int id);
    public Task<BaseResponse<List<ViewListTestViewModel>>> GetDataAsync();
    public Task<BaseResponse<bool>> AddNewTestAsync(AddNewTestViewModel model);
    public Task<BaseResponse<bool>> AddNewResultAsync(AddNewResultViewModel model);
    public Task<BaseResponse<TestPassingViewModel>> GetTestAsync(int id);
    public Task<BaseResponse<int>> CountTestResultAsync(ViewTestViewModel model);
    public Task<BaseResponse<bool>> AddNewQuestionAsync(AddNewQuestionViewModel model);
}
