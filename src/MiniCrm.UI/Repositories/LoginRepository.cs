using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniCrm.UI.Common;
using MiniCrm.UI.Models.DTO_s;
using MiniCrm.UI.Repositories.Interfaces;
using MiniCrm.UI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MiniCrm.UI.Repositories;

public class LoginRepository : ILoginRepository
{
    private readonly ApplicationDbContext _context;

    public LoginRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AuthenticationResponse> LoginAsync(SignInBindingModel model)
    {
        try
        {
            var employee = await _context.Employees
                .Include(x => x.EmployeeRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email.ToLower() == model.Email.ToLower())
                ?? throw new Exception("Не правильный Email/Пароль");

            if (employee.Password != Hasher.GetHash(model.Password, employee.Salt))
                throw new Exception("Не правильный Email/Пароль");

            var claims = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, employee.EmployeeRoles.First().Role.Name),
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddDays(1),
                Subject = claims,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return new AuthenticationResponse()
            {
                AccessToken = stringToken,
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
