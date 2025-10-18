using AsanaFlowDataAccessLayer;
using AsanaFlowDataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsanaFlowWebServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserProgressController : ControllerBase
    {
        private readonly AsanaFlowRepository _repository;

        public UserProgressController()
        {
             _repository = new AsanaFlowRepository();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUserProgressAsync()
        {
            try
            {
                var userProgressList = await _repository.GetAllUserProgressAsync();
                return Ok(userProgressList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("UserId")]
        public async Task<IActionResult> GetUserProgressByUserIdAsync(int userId)
        {
            if (userId <= 0) return BadRequest("Invalid UserId parameter.");

            try
            {
                var userProgressList = await _repository.GetUserProgressByUserIdAsync(userId);
                if (userProgressList == null)
                {
                    return NotFound($"No user progress records found for UserId: {userId}");
                }
                return Ok(userProgressList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("UerProgress")]
        public async Task<IActionResult> AddUserProgressAsync([FromBody] Models.UserProgress userProgress)
        {
            if (userProgress == null) return BadRequest("UserProgress object is null.");
            if (!ModelState.IsValid) return BadRequest("Invalid UserProgress object.");

            try
            {
                UserProgress userProgress1 = new UserProgress
                {
                    UserId = userProgress.UserId,
                    PoseId = userProgress.PoseId,
                    ProficiencyLevel = userProgress.ProficiencyLevel,
                    Notes = userProgress.Notes,
                    LastPracticed = userProgress.LastPracticed
                };
                var createdUserProgress = await _repository.AddUserProgressAsync(userProgress1);
                return Ok("User Progress added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("UserProgress")]
        public async Task<IActionResult> UpdateUserProgressAsync(int progressId, int userId, int poseId, int proficiencyLevel)
        {
            if (progressId <= 0) return BadRequest("Invalid ProgressId parameter.");
            if (userId <= 0) return BadRequest("Invalid UserId parameter.");
            if (poseId <= 0) return BadRequest("Invalid PoseId parameter.");
            try
            {
                var result = await _repository.UpdateUserProgressAsync(progressId,userId,poseId,proficiencyLevel);
                if(result == false) return NotFound($"User Progress with ID {progressId} not found.");
                return Ok("User Progress updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize (Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserProgressAsync(int progressId)
        {
            if (progressId <= 0) return BadRequest("Invalid ProgressId parameter.");

            try
            {
                var result = await _repository.DeleteUserProgressAsync(progressId);
                if (result) return NotFound($"User Progress with ID {progressId} not found.");
                return Ok("User Progress deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
