namespace Service.Interfaces
{
    public interface ICognitoAuthService
    {
        Task RegistrarAsync(string email, string password);
    }
}