using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventPlanning.Interfaces;
using EventPlanning.Models;
using Microsoft.AspNetCore.Identity;
using EventPlanning.Database.Repositories;

namespace EventPlanning.Services
{
	public class AuthService : IAuthService
	{
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
		{
            _configuration = configuration;
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(600),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        public async Task<User> Register(User user, string password)
        {
            if (await _userRepository.EmailExistsAsync(user.Email))
            {
                throw new Exception("Пользователь с таким email уже существует.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            return await _userRepository.CreateAsync(user);
        }
    }
}

