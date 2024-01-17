using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

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

        private static List<CustomerTransactions> Transactions = new List<CustomerTransactions>
    {
        new CustomerTransactions { TransID = 1, CustomerID = 1, TransDate = "2024-01-18", Amount = 100 },
        new CustomerTransactions { TransID = 2, CustomerID = 1, TransDate = "2024-01-19", Amount = 150 },
        new CustomerTransactions { TransID = 3, CustomerID = 2, TransDate = "2024-01-20", Amount = 200 },
        new CustomerTransactions { TransID = 4, CustomerID = 2, TransDate = "2024-02-20", Amount = 500 },
        new CustomerTransactions { TransID = 5, CustomerID = 2, TransDate = "2024-01-20", Amount = 1000 },
        new CustomerTransactions { TransID = 6, CustomerID = 3, TransDate = "2024-02-20", Amount = 2500 },

    };


        [HttpGet("getCustomerTransactions/{customerId}")]
        public IActionResult GetCustomerTransactions(int customerId)
        {
            var transactions = Transactions.Where(t => t.CustomerID == customerId).ToList();

            if (transactions.Count == 0)
            {
                return NotFound("No transactions found for the given customer ID");
            }

            return Ok(transactions);
        }

        [HttpPost("createCustomerTransaction")]
        public IActionResult CreateCustomerTransaction([FromBody] CustomerTransactions transaction)
        {
            // Simulate generating the next transaction ID (replace with your logic)
            transaction.TransID = Transactions.Count + 1;

            Transactions.Add(transaction);

            return Ok(new { Message = "Transaction created successfully", Transaction = transaction });
        }

        [HttpPut("editCustomerTransaction/{transId}")]
        public IActionResult EditCustomerTransaction(int transId, [FromBody] CustomerTransactions editedTransaction)
        {
            var existingTransaction = Transactions.FirstOrDefault(t => t.TransID == transId);

            if (existingTransaction == null)
            {
                return NotFound("Transaction not found");
            }

            // Update the existing transaction with new values
            existingTransaction.TransDate = editedTransaction.TransDate;
            existingTransaction.Amount = editedTransaction.Amount;

            return Ok(new { Message = "Transaction edited successfully", Transaction = existingTransaction });
        }

        [HttpDelete("deleteCustomerTransaction/{transId}")]
        public IActionResult DeleteCustomerTransaction(int transId)
        {
            var existingTransaction = Transactions.FirstOrDefault(t => t.TransID == transId);

            if (existingTransaction == null)
            {
                return NotFound("Transaction not found");
            }

            Transactions.Remove(existingTransaction);

            return Ok(new { Message = "Transaction deleted successfully", Transaction = existingTransaction });
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
        public long Mobile { get; set; }
    }

    public class CustomerTransactions
    {
        public int TransID { get; set; }
        public int CustomerID { get; set; }
        public string TransDate { get; set; }
        public long Amount { get; set; }
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
