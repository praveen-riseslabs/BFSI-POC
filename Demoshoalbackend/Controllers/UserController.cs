using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Demoshoalbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static List<Customer> Customers = new List<Customer>();

        private static List<User> Users = new List<User>
    {
        new User { UserId = 1, Email = "user1@example.com", Password = "user1password", UserType = "user" },
        new User { UserId = 2, Email = "admin1@example.com", Password = "admin1password", UserType = "admin" },

    };

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel loginModel)
        {
            var user = Users.Find(u => u.Email == loginModel.Email && u.Password == loginModel.Password && u.UserType == loginModel.UserType);

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = GenerateJwtToken(user);

            return Ok(new { Message = "Login successful", User = user.Email, Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.UserType)
        };

            var token = new JwtSecurityToken(
                issuer: "shola", // Replace with your own issuer
                audience: "customer", // Replace with your own audience
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("signup")]
        public IActionResult SaveCustomer([FromBody] Customer customer)
        {
            try
            {

                customer.CustomerID = GetNextCustomerId();
                Customers.Add(customer);

                return Ok(new { Message = "Customer saved successfully", Customer = customer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private int GetNextCustomerId()
        {
            // Simulate generating the next customer ID (replace with your logic)
            return Customers.Count + 1;
        }




    }

    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }

    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }

    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }

    public class CustomerTransactions
    {
        public int TransID { get; set; }
        public int CustomerID { get; set; }
        public string TransDate { get; set; }
        public int Amount { get; set; }
    }

    public class CustomerDescripencies
    {
        public int DescripenID { get; set; }
        public int TransID { get; set; }
        public int CustomerID { get; set; }
        public string CreatedDate { get; set; }
        public string Comments { get; set; }
        public string StatusNotes { get; set; }
        public bool isApproved { get; set; }
        public bool IsSynced { get; set; }
        public string ApprovedOn { get; set; }

    }
}
