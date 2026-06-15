using System.Security.Cryptography;
using System.Text;
using FlyJusticeLite.Options;
using Microsoft.Extensions.Options;

namespace FlyJusticeLite.Services;

public sealed class AdminAuthService : IAdminAuthService
{
    private readonly AdminOptions _options;

    public AdminAuthService(IOptions<AdminOptions> options)
    {
        _options = options.Value;
    }

    public Task<bool> ValidateCredentialsAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.Username) ||
            string.IsNullOrWhiteSpace(_options.Password))
        {
            return Task.FromResult(false);
        }

        var usernameMatches = FixedTimeEquals(username.Trim(), _options.Username);
        var passwordMatches = FixedTimeEquals(password, _options.Password);

        return Task.FromResult(usernameMatches && passwordMatches);
    }

    private static bool FixedTimeEquals(string value, string expected)
    {
        var valueBytes = Encoding.UTF8.GetBytes(value);
        var expectedBytes = Encoding.UTF8.GetBytes(expected);

        return valueBytes.Length == expectedBytes.Length &&
            CryptographicOperations.FixedTimeEquals(valueBytes, expectedBytes);
    }
}
