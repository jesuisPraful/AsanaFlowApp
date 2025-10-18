using AsanaFlowDataAccessLayer;
using AsanaFlowDataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsanaFlowWebServices.Services;
using AsanFlowEbServices.Services;
using Microsoft.AspNetCore.Authorization;



namespace AsanaFlowWebServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AsanaFlowRepository _repository;
        private readonly EmailServices _emailServices;
        private readonly JwtService _jwtService;
        public UserController(EmailServices emailServices, JwtService jwtService)
        {
            _repository =  new AsanaFlowRepository();
            _emailServices = emailServices;
            _jwtService = jwtService;

        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                // Get the logged-in user's ID from JWT claims
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("Invalid token or user not found.");

                int userId = int.Parse(userIdClaim);

                var user = await _repository.GetUserByIdAsync(userId);

                if (user == null)
                    return NotFound("User not found.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _repository.GetAllUsersAsync();
                if (users == null)
                {
                    return NotFound("No users found.");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [Authorize]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetUserById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid user ID.");

            try
            {
                var user = await _repository.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetUsersByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Invalid user name.");

            try
            {
                var users = await _repository.GetUserAsync(name);
                if (users == null)
                    return NotFound($"User with name {name} not found.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUsersByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Invalid email.");

            try
            {
                var users = await _repository.GetUserByEmailAsync(email);
                if (users == null)
                    return NotFound($"User with email {email} not found.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost("SignUp")]
        public async Task<IActionResult> UserSignup([FromBody] Models.User user)
        {
            if (string.IsNullOrEmpty(user.Email) ||
                string.IsNullOrEmpty(user.Name) ||
                string.IsNullOrEmpty(user.PasswordHash))
                return BadRequest("Name, email and password cannot be null");

            var existingUser = await _repository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
                return BadRequest("User already exists with this email.");

            try
            {

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

                User userObj = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    PasswordHash = hashedPassword,
                    Role = user.Role,
                    CreatedAt = DateTime.Now,
                    EmailVerified = false,
                    FailedLoginAttempts = 0,
                    MfaEnabled = false
                };


                TempStore.PendingUsers[user.Email] = userObj;


                var otp = new Random().Next(100000, 999999).ToString();


                TempStore.EmailOtps[user.Email] = (otp, DateTime.Now.AddMinutes(5));


                await _emailServices.SendEmailAsync(
                    user.Email,
                    "AsanaFlow Email Verification OTP",
                    $"<h2>Hello {user.Name}</h2><p>Your OTP is: <b>{otp}</b></p><p>Valid for 5 minutes.</p>"
                );

                return Ok("User created successfully. Please verify OTP sent to your email.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin(string email,string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return BadRequest("Email and password cannot be null");

            try
            {
                var user = await _repository.GetUserByEmailAsync(email);
                if (user == null || user.PasswordHash != password)
                    return Unauthorized("Invalid credentials");

                
                //Generate JWT token using claims
                var token = _jwtService.GenerateToken(user.UserId,user.Email,user.Role);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost("VerifySignUpOtp")]
        public async Task<IActionResult> VerifySignUpOtp([FromBody] Models.VerifyOtpRequest request)
        {
            // Check if OTP exists  
            if (!TempStore.EmailOtps.ContainsKey(request.Email)) return BadRequest("OTP not found or expired");
            try
            {


                var (storedOtp, expiry) = TempStore.EmailOtps[request.Email];
                if (DateTime.Now > expiry)
                    return BadRequest("OTP expired");

                if (storedOtp != request.Otp)
                    return BadRequest("Invalid OTP");

                // Check if user data exists
                if (!TempStore.PendingUsers.ContainsKey(request.Email))
                    return BadRequest("User data not found. Please re-register.");

                var userData = TempStore.PendingUsers[request.Email];

                // Mark email verified
                userData.EmailVerified = true;
                userData.CreatedAt = DateTime.Now;

                // Add user to the database
                var result = await _repository.AddUserAsync(userData);
                if (!result)
                    return BadRequest("Failed to create user after OTP verification.");

                // Cleanup temporary stores
                TempStore.EmailOtps.TryRemove(request.Email, out _);
                TempStore.PendingUsers.TryRemove(request.Email, out _);

                return Ok("Email verified and account created successfully!");
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("RequestOtpForReset")]
        public async Task<IActionResult> RequestOtpForResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("Email cannot be null or empty");

            try
            {
                var user = await _repository.GetUserByEmailAsync(email);
                if (user == null) return NotFound("User not found.");

                var otp = new Random().Next(100000, 999999).ToString();
                TempStore.EmailOtps[user.Email] = (otp, DateTime.Now.AddMinutes(5));

                await _emailServices.SendEmailAsync(
                    user.Email,
                    "AsanaFlow Email Verification OTP",
                    $"<h2>Hello {user.Name}</h2><p>Your OTP is: <b>{otp}</b></p><p>Valid for 5 minutes.</p>"
                );
                return Ok("Opt send!");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to send otp");
            }
        }



        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] Models.ResetPasswordRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Otp) ||
                string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest("Email, OTP, and new password are required.");
            }

            try
            {
                // Validate OTP
                if (!TempStore.EmailOtps.TryGetValue(request.Email, out var otpData))
                    return BadRequest("OTP not found or expired.");

                var (storedOtp, expiry) = otpData;
                if (DateTime.UtcNow > expiry)
                {
                    TempStore.EmailOtps.TryRemove(request.Email, out _);
                    return BadRequest("OTP has expired. Please request a new one.");
                }

                if (storedOtp != request.Otp)
                    return BadRequest("Invalid OTP.");

                // Fetch user from DB
                var user = await _repository.GetUserByEmailAsync(request.Email);
                if (user == null)
                    return NotFound("User not found.");

                // Hash and update password
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                var result = await _repository.UpdatePasswordAsync(request.Email, passwordHash);

                if (result == -99)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to reset password.");

                // Cleanup OTP
                TempStore.EmailOtps.TryRemove(request.Email, out _);

                return Ok("Password has been reset successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize]
        [HttpPut("{newUsername}")]
        public async Task<IActionResult> UpdateUserNameAsync(string newUsername, String email)
        {
            if (newUsername == null || email == null)
                return BadRequest("New Username,email and password can not be null");

            try
            {
                int result = await _repository.UpdateUserNameAsync(newUsername, email);
                if (result == 1) return Ok("Name updated successfully");

                else if (result == -1)
                {
                    return BadRequest("User not found!");
                }
                else
                {
                    return BadRequest("Menu item could not be updated!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid user ID.");
            try
            {
                int result = await _repository.DeleteUserAsync(userId);
                if (result == 1) return Ok("User deleted successfully");

                else if (result == -1)
                {
                    return BadRequest("User not found!");
                }
                else
                {
                    return BadRequest("User could not be deleted!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}