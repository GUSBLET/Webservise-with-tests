namespace BusinessLogicLayer.Services;

public class AccountService : IAccountService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<Profile> _profileRepository;

    public AccountService(
        IBaseRepository<User> userRepository, 
        IBaseRepository<Profile> profileRepository)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
    }

    public Task<HttpStatusCode> Registration()
    {
        throw new NotImplementedException();
    }
}
