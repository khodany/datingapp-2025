using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenServices tokenServices) : BaseApiController
{
    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDtos)
    {
       if(await EmailExists(registerDtos.Email)) return BadRequest("Email taken");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            DisplayName = registerDtos.DisplayName,
            Email= registerDtos.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDtos.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);

        await context.SaveChangesAsync();

        return user.ToDto(tokenServices);

    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x=>x.Email == loginDto.Email);

        if(user==null) return Unauthorized("Invalid email address");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for(var i= 0; i <computedHash.Length; i++)
        {
            if(computedHash[i]!= user.PasswordHash[i]) return Unauthorized("Invalid password");
        }
        return user.ToDto(tokenServices);

    }

    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x=>x.Email.ToLower() == email.ToLower());
    }
    
}
