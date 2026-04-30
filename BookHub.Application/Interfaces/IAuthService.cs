using BookHub.Application.DTOs.Auth;


namespace BookHub.Application.Interfaces;


public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}