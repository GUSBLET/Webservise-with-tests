namespace BusinessLogicLayer.Interfaces;

public interface IAccountService
{
    public Task<BaseResponse<ClaimsIdentity>> RegistrationAsync(RegistrationViewModel model);
    public Task<BaseResponse<ClaimsIdentity>> LoginAsync(LoginViewModel model);
    public Task<User> GetDataForMailByEmailAsync(string email);
    public Task<ProfileViewModel> GetProfileByEmailAsync(string email);
    public Task<User> GetUserByIdAsync(int id);
    public Task<HttpStatusCode> UpdateManagerRoleAsync(User model);
    public Task<HttpStatusCode> UpdateAdminRoleAsync(User model);
    public Task<HttpStatusCode> DeleteAdminAsync(int id);
    public Task<HttpStatusCode> DeleteManagerAsync(int id);
    public Task<HttpStatusCode> ConfirmEmailAsync(int id,string code);
    public Task<HttpStatusCode> ResetPasswordAsync(int id, string password);
    public Task<HttpStatusCode> UpdateProfileAsync(ProfileViewModel model);
    public Task<BaseResponse<User>> SendResetPasswordCodeAsync(SendResetPasswordRequistViewModel model);
    public Task<BaseResponse<List<User>>> GetListOfUsersAsync();
}
