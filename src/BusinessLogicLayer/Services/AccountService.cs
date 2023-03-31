using Azure;

namespace BusinessLogicLayer.Services;

public class AccountService : IAccountService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<Profile> _profileRepository;
    private readonly IBaseRepository<ProfileImprovingData> _profileImprovingDataRepository;
    private readonly IBaseRepository<ImprovingData> _improvingDataRepository;
    private readonly IBaseRepository<Test> _testRepository;

    public AccountService(
        IBaseRepository<User> userRepository, 
        IBaseRepository<Profile> profileRepository,
        IBaseRepository<ProfileImprovingData> profileImprovingDataRepository,
        IBaseRepository<ImprovingData> improvingDataRepository,
        IBaseRepository<Test> testRepository)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
        _profileImprovingDataRepository = profileImprovingDataRepository;
        _improvingDataRepository = improvingDataRepository;
        _testRepository = testRepository;
    }

    public async Task<BaseResponse<ClaimsIdentity>> LoginAsync(LoginViewModel model)
    {
        try
        {
            var result = await (from user in _userRepository.Select()
                         where user.Email == model.Email 
                         select new AuthorizationModelDTO 
                         { 
                             Email = user.Email,
                             Password = user.Password,
                             EmailConfirmed = user.EmailConfirmed,
                             Role = user.Role
                         }).FirstOrDefaultAsync();
            if (result is null)
                return new BaseResponse<ClaimsIdentity>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Description = "User is not found"
                };

            if(result.EmailConfirmed)
            {
                if(result.Password == HashPasswordHelper.HashPassowrd(model.Password))
                {
                    var response = Authenticate(result);
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Data = response,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                return new BaseResponse<ClaimsIdentity>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Description = "Password is wrong"
                };
            }
            return new BaseResponse<ClaimsIdentity>
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Description = "Email didn't confirm"
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public Task<HttpResponseMessage> LogoutAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponse<ClaimsIdentity>> RegistrationAsync(RegistrationViewModel model)
    {
        try
        {
            var userExist = await (from p in _userRepository.Select()
                                   where p.Email == model.Email
                                   select p).FirstOrDefaultAsync();
            if (userExist != null)
                return new BaseResponse<ClaimsIdentity>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                };

            if(MaskPassword(model.Password))
            {
                var lastId = await _userRepository.Select().FirstOrDefaultAsync() is null ? 1 :
                    _userRepository.Select().MaxAsync(p => p.Id).Result + 1;
                var newAccount = new User()
                {
                    Email = model.Email,
                    Password = HashPasswordHelper.HashPassowrd(model.Password),
                    Role = Role.User,
                    EmailConfirmedToken = Guid.NewGuid(),
                    EmailConfirmed = false,
                    ProfileId = lastId,
                };
                var newProfile = new Profile()
                {
                    FullName = "Name",
                    Avatar = new byte[0],
                    Year = new DateOnly(2000, 12, 20),
                    UserId = lastId,
                };
                await _profileRepository.Add(newProfile);
                if (await _userRepository.Add(newAccount))
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        StatusCode = HttpStatusCode.OK,
                    };

                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Account didn't add, program error",
                    StatusCode = HttpStatusCode.InternalServerError,
                };
            }
            return new BaseResponse<ClaimsIdentity> 
            { 
                StatusCode = HttpStatusCode.Unauthorized, 
                Description = "Password has to contain chars and numbers"
            };
            
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<HttpStatusCode> ConfirmEmailAsync(int id, string code)
    {
        try
        {
            var user = await (from p in _userRepository.Select()
                              where p.Id == id
                              select p).FirstOrDefaultAsync();

            if (user == null || user.EmailConfirmedToken.ToString() != code)
                return HttpStatusCode.BadRequest;

            if (user.Id == id && user.EmailConfirmedToken.ToString() == code)
            {
                user.EmailConfirmed = true;
                if (await _userRepository.Update(user))
                    return HttpStatusCode.OK;
            }

            return HttpStatusCode.BadRequest;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<User>> SendResetPasswordCodeAsync(SendResetPasswordRequistViewModel model)
    {
        try
        {
            var user = await GetDataForMailByEmailAsync(model.Email);
            if (user is null)
                return new BaseResponse<User>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Description = "User dosen't exist"
                };

            return new BaseResponse<User>()
            {
                StatusCode = HttpStatusCode.OK,
                Data = user
            };
            
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<HttpStatusCode> ResetPasswordAsync(int id, string password)
    {
        try
        {
            var user = await (from p in _userRepository.Select()
                        where p.Id == id
                        select p).FirstOrDefaultAsync();
            if (user is null)
                return HttpStatusCode.BadRequest;
            
            if(MaskPassword(password))
            {
                user.Password = HashPasswordHelper.HashPassowrd(password);
                if(await _userRepository.Update(user))
                    return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<User> GetDataForMailByEmailAsync(string email)
    {
        try
        {
            var user = await _userRepository.Select().Where(x => x.Email == email)
                .Include(x => x.Profile).FirstOrDefaultAsync();
            if (user is null)
                return null;
            
            return user;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<ProfileViewModel> GetProfileByEmailAsync(string email)
    {
        try
        {
            List<ImprovingData> improvingDatas = new List<ImprovingData>();
            List<Test> tests = new List<Test>();
            var profile = await _profileRepository.Select().Include(x => x.User).FirstOrDefaultAsync();
            
            var profileDataBuffer = _profileRepository.Select().Include(x => x.ProfileImprovingDatas.Where(x => x.ProfileId == profile.Id)).ToList();
            foreach (var item in profileDataBuffer)
            {
                if (item.ProfileImprovingDatas != null)
                {
                    foreach (var workItem in item.ProfileImprovingDatas)
                    {
                        var datas = await _improvingDataRepository.Select().Where(x => x.Id == workItem.ImprovingDataId).FirstOrDefaultAsync();
                        improvingDatas.Add(datas);
                    }
                }
                
                if(item.ProfileTests != null)
                {
                    foreach (var workItem in item.ProfileTests)
                    {
                        var datas = await _testRepository.Select().Where(x => x.Id == workItem.TestId).FirstOrDefaultAsync();
                        tests.Add(datas);
                    }
                }
            }
            

            return new ProfileViewModel
            {
                FullName = profile.FullName,
                AvatarShow = profile.Avatar,
                Id = profile.Id,
                User = profile.User,
                Year = profile.Year,
                ImprovingDatas = improvingDatas,
                Test = tests
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<HttpStatusCode> UpdateProfileAsync(ProfileViewModel model)
    {
        try
        {
            var profile = await _profileRepository.Select().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            profile.Year = model.Year;
            profile.FullName = model.FullName;
            if(model.Avatar != null)
                profile.Avatar = PackingPhoto(model.Avatar);
            if(await _profileRepository.Update(profile))
                return HttpStatusCode.OK;
            return HttpStatusCode.BadRequest;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<BaseResponse<List<User>>> GetListOfUsersAsync()
    {
        try
        {
            var user = await _userRepository.Select().ToListAsync();
            if(user != null)
                return new BaseResponse<List<User>>
                {
                    Data = user,
                    StatusCode = HttpStatusCode.OK
                };

            return new BaseResponse<List<User>>
            {
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    private bool MaskPassword(string password)
    {
        bool onlyEn = true; // variable to check for only en key
        bool number = false; // variable to check for exist number
        for (int i = 0; i < password.Length; i++)
        {
            if (password[i] >= 'А' && password[i] <= 'Я') onlyEn = false;
            if (password[i] >= '0' && password[i] <= '9') number = true;
        }
        if (onlyEn && number)
            return true;
        return false;
    }

    private ClaimsIdentity Authenticate(AuthorizationModelDTO user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };
        return new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    }

    private byte[] PackingPhoto(IFormFile file)
    {
        using (var binaryReader = new BinaryReader(file.OpenReadStream()))
        {
            var photo = binaryReader.ReadBytes((int)file.Length);
            return photo;
        }
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _userRepository.Select().Where(x => x.Id == id)
                .Include(x => x.Profile).FirstOrDefaultAsync();
            if (user is null)
                return null;

            return user;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<HttpStatusCode> UpdateManagerRoleAsync(User model)
    {
        try
        {
            var res = await _userRepository.Select().Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if (model.Role == Role.Admin || model.Role == Role.Manager || res.Role == Role.Admin || res.Role == Role.Manager)
                return HttpStatusCode.BadRequest;

            res.Role = model.Role;
            if (await _userRepository.Update(res))
                return HttpStatusCode.OK;

            return HttpStatusCode.BadRequest;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<HttpStatusCode> UpdateAdminRoleAsync(User model)
    {
        try
        {
            var res = await _userRepository.Select().Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if(res is null) 
                return HttpStatusCode.BadRequest;

            if (model.Role == Role.Admin || res.Role == Role.Admin)
                return HttpStatusCode.BadRequest;

            res.Role = model.Role;
            if (await _userRepository.Update(res))
                return HttpStatusCode.OK;

            return HttpStatusCode.BadRequest;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<HttpStatusCode> DeleteAdminAsync(int id)
    {
        try
        {
            var res = await _userRepository.Select().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (res is null)
                return HttpStatusCode.BadRequest;

            if (res.Role == Role.Admin)
                return HttpStatusCode.BadRequest;

            if (await _userRepository.Delete(res))
                return HttpStatusCode.OK;

            return HttpStatusCode.BadRequest;

        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<HttpStatusCode> DeleteManagerAsync(int id)
    {
        try
        {
            var res = await _userRepository.Select().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (res is null)
                return HttpStatusCode.BadRequest;

            if (res.Role == Role.Admin || res.Role == Role.Manager)
                return HttpStatusCode.BadRequest;

            if (await _userRepository.Delete(res))
                return HttpStatusCode.OK;

            return HttpStatusCode.BadRequest;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
