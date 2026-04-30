using BookHub.Application.DTOs.Auth;
using BookHub.Application.Interfaces;
using BookHub.Domain.Entities;
using BookHub.Domain.Enums;
using BookHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookHub.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // 1. Check if email is already in use
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            throw new Exception("Email already in use.");
        }

        // 2. Create company 
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.CompanyName,
            Slug = request.Slug,
            CreatedAt = DateTime.UtcNow
        };
        _context.Companies.Add(company);

        // Hash password and create user
        var user = new User
        {
            Id = Guid.NewGuid(),
            CompanyId = company.Id,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = UserRole.Admin, // First registered user is admin of the company
            CreatedAt = DateTime.UtcNow
        };
        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        // Generate JWT token and return response
        var token = GenerateJwtToken(user);
        return new AuthResponse { Token = token, Email = user.Email, Role = user.Role.ToString() };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // 1. Find user by email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)        {
            throw new Exception("Invalid email or password.");
        }

        // 2. Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new Exception("Invalid email or password.");
        }

        // 3. Generate JWT token and return response
        var token = GenerateJwtToken(user);
        return new AuthResponse { Token = token, Email = user.Email, Role = user.Role.ToString() };
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("companyId", user.CompanyId.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())

        };

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds,
            claims: claims
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
        
    }
}