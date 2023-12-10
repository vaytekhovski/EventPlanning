using System;
using EventPlanning.Models;

namespace EventPlanning.Interfaces
{
	public interface IAuthService
	{
        Task<User> Register(User user, string password);
        Task<string> Login(string email, string password);
    }
}

