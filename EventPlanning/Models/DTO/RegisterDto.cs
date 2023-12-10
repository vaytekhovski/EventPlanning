using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanning.Models.DTO
{
	public class RegisterDto
	{
        public string Username { get; set; }
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}

