using System.Net;

namespace BusinessLogicLayer.Interfaces;

public interface IAccountService
{
    public Task<HttpStatusCode> Registration();
}
