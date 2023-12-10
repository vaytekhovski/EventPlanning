using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanning.Models.DTO
{
	public class LoginDto
	{
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}

