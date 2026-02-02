using Assessment.Application.Dtos.Auth;

namespace Assessment.Application.Abstractions;

public interface ITokenService
{
    Task<LoginResponseDto> LoginAsync(string email, string password, CancellationToken ct = default);
}
