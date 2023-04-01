using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services;

public class ImprovingDataService : IImprovingDataService
{
    private readonly IBaseRepository<ImprovingData> _improvingDataRepository;
    private readonly IBaseRepository<Profile> _profileRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<ProfileImprovingData> _profileImprovingDatasRepository;

    public ImprovingDataService(
        IBaseRepository<ImprovingData> improvingDataRepository,
        IBaseRepository<Profile> profileRepository,
        IBaseRepository<User> userRepository,
        IBaseRepository<ProfileImprovingData> profileImprovingDatasRepository)
    {
        _improvingDataRepository = improvingDataRepository;
        _profileRepository = profileRepository;
        _userRepository = userRepository;
        _profileImprovingDatasRepository = profileImprovingDatasRepository;
    }

    public async Task<HttpStatusCode> AddNewItemAsync(AddNewImprovingDataViewModel model)
    {
        try
        {
            var data = new ImprovingData
            {
                Title = model.Title,
                Description = model.Description,
                Information = model.Information,
                Author = await (from p in _userRepository.Select()
                                where p.Email == model.Email
                                select p).FirstOrDefaultAsync()
            };

            if(await _improvingDataRepository.Add(data))
                return HttpStatusCode.OK;

            return HttpStatusCode.BadRequest;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<List<ViewListImprovingDataViewModel>>> GetDataAsync()
    {
        try
        {
            var result = await (from impr in _improvingDataRepository.Select()
                          join c in _userRepository.Select() on impr.Author.Id equals c.Id
                          select new ViewListImprovingDataViewModel
                          {
                              Author = impr.Author,
                              Description = impr.Description,
                              Title = impr.Title,
                              Id = impr.Id
                          }).ToListAsync();
            if(result.Count > 0)
                return new BaseResponse<List<ViewListImprovingDataViewModel>>
                {
                    Data = result,
                    StatusCode = HttpStatusCode.OK
                };

            return new BaseResponse<List<ViewListImprovingDataViewModel>>
            {
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<BaseResponse<ViewImprovingDataViewModel>> GetDataByIdAsync(int id)
    {
        try
        {
            var result = await (from p in _improvingDataRepository.Select()
                          where p.Id == id
                          select new ViewImprovingDataViewModel
                          {
                              Title = p.Title,
                              Description = p.Description,
                              Information = p.Information
                          }).FirstOrDefaultAsync();

            return new BaseResponse<ViewImprovingDataViewModel>
            {
                Data = result
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<HttpStatusCode> SetToReadDataAsync(string email, int dataId)
    {
        try
        {
            var profile = (from p in _profileRepository.Select()
                           where p.Id == _userRepository.Select().Where(x => x.Email == email).FirstOrDefault().Id
                           select p).FirstOrDefault();
            var data = new ProfileImprovingData
            {
                ProfileId = profile.Id,
                ImprovingDataId = dataId
            };
            var re = await _profileImprovingDatasRepository.Select()
                .Where(x => x.ImprovingDataId == dataId && x.ProfileId == profile.Id)
                .FirstOrDefaultAsync();
            if (re == null)
            {
                if (await _profileImprovingDatasRepository.Add(data))
                    return HttpStatusCode.OK;
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.OK;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
