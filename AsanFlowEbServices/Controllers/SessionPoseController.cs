using AsanaFlowDataAccessLayer;
using AsanaFlowDataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsanaFlowWebServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SessionPoseController : ControllerBase
    {
        private readonly AsanaFlowRepository _repository;

        public SessionPoseController()
        {
            _repository = new AsanaFlowRepository();
        }

        [Authorize (Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllSessionPoses()
        {
            try
            {
                var sessionPoses = await _repository.GetAllSessionPosesAsync();
                if (sessionPoses == null || sessionPoses.Count == 0)
                {
                    return NotFound("No session poses found.");
                }
                return Ok(sessionPoses);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetSessionPoseByIdAsync(int sessionPoseId)
        //{
        //    if (sessionPoseId <= 0) return BadRequest("Invalid session pose ID.");
        //    try
        //    {
        //        var sessionPose = await _repository.GetSessionPoseByIdAsync(sessionPoseId);
        //        if (sessionPose == null)
        //        {
        //            return NotFound($"Session pose with ID {sessionPoseId} not found.");
        //        }
        //        return Ok(sessionPose);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        [Authorize]
        [HttpGet("SessionId")]
        public async Task<IActionResult> GetSessionPosesBySessionIdAsync(int sessionId)
        {
            if (sessionId <= 0) return BadRequest("Invalid session ID.");

            try
            {
                var sessionPoses = await _repository.GetSessionPosesBySessionIdAsync(sessionId);
                if (sessionPoses == null || sessionPoses.Count == 0)
                {
                    return NotFound($"No session poses found for session ID {sessionId}.");
                }
                return Ok(sessionPoses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error:{ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("poseId")]
        public async Task<IActionResult> GetSessionPosesByPoseIdAsync(int poseId)
        {
            if (poseId <= 0) return BadRequest("Invalid pose ID.");
            try
            {
                var sessionPoses = await _repository.GetSessionPosesByPoseIdAsync(poseId);
                if (sessionPoses == null || sessionPoses.Count == 0)
                {
                    return NotFound($"No session poses found for pose ID {poseId}.");
                }
                return Ok(sessionPoses);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }

        [Authorize]
        [HttpPost("Session")]
        public async Task<IActionResult> AddSessionPose([FromBody] Models.SessionPose sessionPose)
        {
            if (sessionPose == null) return BadRequest("Session pose is null.");
            if (!ModelState.IsValid) return BadRequest("Invalid session pose data.");

            try
            {
                SessionPose sessionPose1 = new SessionPose
                {
                    SessionId = sessionPose.SessionId,
                    PoseId = sessionPose.PoseId,
                    Duration = sessionPose.Duration
                };
                var createdSessionPose = await _repository.AddSessionPoseAsync(sessionPose1);
                if(createdSessionPose == false)
                {
                    return NotFound("Session or Pose not found. Cannot create SessionPose.");
                }
                return Ok("Session pose created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("{sessionPoseId}")]
        public async Task<IActionResult> DeleteSessionPose(int sessionPoseId)
        {
            if (sessionPoseId <= 0) return BadRequest("Invalid session pose ID.");

            try
            {
                var deleted = await _repository.DeleteSessionPoseAsync(sessionPoseId);
                if (deleted==false)
                {
                    return NotFound($"Session pose with ID {sessionPoseId} not found.");
                }
                return Ok("Session pose deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("BySessionId/{sessionId}")]
        public async Task<IActionResult> DeleteSessionPosesBySessionIdAsync(int sessionId)
        {
            if (sessionId <= 0) return BadRequest("Invalid session ID.");
            try
            {
                var deleted = await _repository.DeleteSessionPosesBySessionIdAsync(sessionId);
                if (deleted == false)
                {
                    return NotFound($"No session poses found for session ID {sessionId}.");
                }
                return Ok("Session poses deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error.{ex.Message}");
            }
        }

    }
}
