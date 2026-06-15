namespace FlyJusticeLite.Services;

public interface IAdminAuthService
{
    Task<bool> ValidateCredentialsAsync(string username, string password, CancellationToken cancellationToken = default);
}
