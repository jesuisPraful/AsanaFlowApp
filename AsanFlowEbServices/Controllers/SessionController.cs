using AsanaFlowDataAccessLayer;
using AsanaFlowDataAccessLayer.Models;
using AsanaFlowWebServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsanaFlowWebServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly AsanaFlowRepository _repository;

        public SessionController()
        {
            _repository = new AsanaFlowRepository();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async  Task<IActionResult> GetAllSessionsAsync()
        {
            try
            {
                var sessions = await _repository.GetAllSessionsAsync();
                if (sessions == null || sessions.Count == 0)
                {
                    return NotFound();  
                }
                return Ok(sessions);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("sessionId")]
        public async Task<IActionResult> GetSessionsBySessionIdIdAsync(int sessionId)
        {
            if (sessionId <= 0) return BadRequest("Invalid session ID");

            try
            {
                var session = await _repository.GetSessionBySessionIdAsync(sessionId);
                if (session == null) return NotFound("Session Not Found");

                return Ok(session);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("User")]
        public async Task<IActionResult> GetSessionsByUserIdAsync(int sessionId, int userId)
        {
            if (userId <= 0) return BadRequest("Invalid user ID");
            if (sessionId <= 0) return BadRequest("Invalid session ID");

            try
            {
                var session = await _repository.GetSessionsByUserIdAsync(sessionId, userId);
                if (session == null) return NotFound("Session Not Found");
                return Ok(session);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [Authorize]
        [HttpGet("Session")]
        public async Task<IActionResult> AddSessionAsync([FromBody] Models.Session session)
        {
            if (session == null) return BadRequest("Session is null");
            if (!ModelState.IsValid) return BadRequest("Invalid Session object.");

            try
            {
                AsanaFlowDataAccessLayer.Models.Session session1 = new AsanaFlowDataAccessLayer.Models.Session
                {
                    SessionId = session.SessionId,
                    UserId = session.UserId,
                    Date = session.Date,
                    TotalDuration = session.TotalDuration
                };

                var result = await _repository.AddSessionAsync(session1);

                if (result == false) return NotFound("Session Not Added");
                return Ok("Session added Successfully");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new session record");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{sessionId}")]
        public async Task<IActionResult> DeleteSessionAsync(int sessionId)
        {
            if(sessionId<=0) return BadRequest("Invalid session ID");
            try
            {
                var result = await _repository.DeleteSessionAsync(sessionId);
                if (result == false) return NotFound("Session Not Found");
                return Ok("Session Deleted Successfully");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting session record");
            }
        }
    }
}
