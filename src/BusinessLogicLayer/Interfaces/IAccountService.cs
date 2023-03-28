﻿namespace BusinessLogicLayer.Interfaces;

public interface IAccountService
{
    public Task<BaseResponse<ClaimsIdentity>> RegistrationAsync(RegistrationViewModel model);
    public Task<BaseResponse<ClaimsIdentity>> LoginAsync(LoginViewModel model);
    public Task<User> GetDataForMailByEmailAsync(string email);
    public Task<HttpStatusCode> ConfirmEmailAsync(int id,string code);
    public Task<HttpStatusCode> ResetPasswordAsync(int id, string password);
    public Task<BaseResponse<User>> SendResetPasswordCodeAsync(SendResetPasswordRequistViewModel model);
    public Task<HttpResponseMessage> LogoutAsync();
}
