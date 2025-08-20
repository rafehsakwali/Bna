using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YourProject.Models;

namespace YourProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly string connectionString;
        private readonly IConfiguration _config;

        public UsersController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string is missing.");
            _config = configuration;
        }

        // -------------------- REGISTER --------------------
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserModel user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM sign WHERE Username = @Username";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Username", user.Username ?? string.Empty);
                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                        return BadRequest(new { success = false, message = "Nom d'utilisateur déjà pris" });
                }

                string insertQuery = "INSERT INTO sign (Username, Password, Email) VALUES (@Username, @Password, @Email)";
                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@Username", user.Username ?? string.Empty);
                    insertCmd.Parameters.AddWithValue("@Password", user.Password ?? string.Empty);
                    insertCmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    insertCmd.ExecuteNonQuery();
                }

                return Ok(new { success = true, message = "Inscription réussie !" });
            }
        }

        // -------------------- LOGIN --------------------
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserModel user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM sign WHERE Username = @Username AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Password", user.Password ?? string.Empty);

                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        string token = GenerateJwtToken(user.Username);
                        TokenStorage.Token = token;
                        return Ok(new { success = true, token });
                    }
                    else
                    {
                        return Unauthorized(new { success = false, message = "Identifiants incorrects" });
                    }
                }
            }
        }


       private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token valide 1h
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


    public static class TokenStorage
    {
        // Token partagé par toute l'application
        public static string Token { get; set; }
    }


}
