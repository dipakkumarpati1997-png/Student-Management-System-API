using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Student_Management_System.Helpers;
using Student_Management_System.Models;
using Student_Management_System.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Student_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _IStudentService;
        private readonly ILogger<StudentController> _logger;
        private readonly IConfiguration _configuration;

        public StudentController(IStudentService IStudentService, ILogger<StudentController> logger, IConfiguration Configuration)
        {
            _IStudentService = IStudentService;
            _logger = logger;
            _configuration = Configuration;
        }

        // ================= GET =================
        [Authorize]
        [HttpGet("StudetDetailsGet")]
        public IActionResult StudetDetailsGet()
        {
            _logger.LogInformation("StudetDetailsGet API called");

            var data = _IStudentService.StudetDetailsGet();

            _logger.LogInformation("Student data fetched successfully");

            return Ok(new
            {
                success = true,
                message = "Data fetched successfully",
                data = data
            });
        }

        // ================= ADD =================
        [Authorize]
        [HttpPost("AddStudentDetails")]
        public IActionResult AddStudentDetails(StudentAddModel oStudentAddModel)
        {
            _logger.LogInformation("Adding student: {Name}", oStudentAddModel.Student_Name);

            var result = _IStudentService.AddStudentDetails(oStudentAddModel);

            _logger.LogInformation("Student added successfully");

            return Ok(result);
        }

        // ================= UPDATE =================
        [Authorize]
        [HttpPut("UpdateStudentDetails")]
        public IActionResult UpdateStudentDetails(StudentEditModel oStudentEditModel)
        {
            _logger.LogInformation("Updating student ID: {Id}", oStudentEditModel.Student_Id);

            var result = _IStudentService.UpdateStudentDetails(oStudentEditModel);

            _logger.LogInformation("Student updated successfully");

            return Ok(result);
        }

        // ================= DELETE =================
        [Authorize]
        [HttpDelete("DeleteStudentDetails/{id}")]
        public IActionResult DeleteStudentDetails(int id)
        {
            _logger.LogInformation("Deleting student ID: {Id}", id);

            var result = _IStudentService.DeleteStudentDetails(id);

            _logger.LogInformation("Student deleted successfully");

            return Ok(result);
        }

       

        // ================= TOKEN =================
        private string GenerateToken(string username)
        {
            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

      //==============Login==============================

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            _logger.LogInformation("Login attempt for user: {User}", model.Username);

            var hashedPassword = PasswordHelper.HashPassword(model.Password);

            var user = _IStudentService.ValidateUser(model.Username, hashedPassword);

            if (user != null)
            {
                var token = GenerateToken(model.Username);

                HttpContext.Session.SetString("JWToken", token);

                _logger.LogInformation("Login successful for user: {User}", model.Username);

                return Ok(new
                {
                    message = "Login successful"
                });
            }

            _logger.LogWarning("Invalid login attempt for user: {User}", model.Username);

            return Unauthorized("Invalid username or password");
        }

        // ================= ADD USER =================
        [HttpPost("AddUser")]
        public IActionResult AddUser(string username, string password)
        {
            _logger.LogInformation("Adding User: {Username}", username);

            var result = _IStudentService.AddUser(username, password);

            _logger.LogInformation("User added successfully");

            return Ok(result);
        }

    }
}