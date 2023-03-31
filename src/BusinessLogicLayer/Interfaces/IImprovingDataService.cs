namespace BusinessLogicLayer.Interfaces;

public interface IImprovingDataService
{
    public Task<BaseResponse<List<ViewListImprovingDataViewModel>>> GetDataAsync();
    public Task<BaseResponse<ViewImprovingDataViewModel>> GetDataByIdAsync(int id);
    public Task<HttpStatusCode> SetToReadDataAsync(string email, int dataId);
    public Task<HttpStatusCode> AddNewItemAsync(AddNewImprovingDataViewModel model);
}
