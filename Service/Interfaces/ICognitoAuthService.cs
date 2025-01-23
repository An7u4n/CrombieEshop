using Amazon.CognitoIdentityProvider.Model;

namespace Service.Interfaces
{
    public interface ICognitoAuthService
    {
        Task RegistrarAsync(string email, string password);
        Task ConfirmarRegistroAsync(string userName, string code);
        Task<string> IniciarSesion(string userName, string password);
    }
}