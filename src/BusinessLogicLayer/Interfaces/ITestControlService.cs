namespace BusinessLogicLayer.Interfaces;

public interface ITestControlService
{
    public Task<BaseResponse<List<ViewListTestViewModel>>> GetDataAsync();
    public Task<BaseResponse<bool>> AddNewTestAsync(AddNewTestViewModel model);
    public Task<BaseResponse<bool>> AddNewQuestionAsync(AddNewQuestionViewModel model);
}
