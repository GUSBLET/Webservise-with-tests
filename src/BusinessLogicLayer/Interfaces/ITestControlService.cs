namespace BusinessLogicLayer.Interfaces;

public interface ITestControlService
{
    public Task<BaseResponse<List<ViewListTestViewModel>>> GetDataAsync();
}
