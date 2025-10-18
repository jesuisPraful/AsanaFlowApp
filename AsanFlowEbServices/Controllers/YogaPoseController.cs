using AsanaFlowDataAccessLayer;
using AsanaFlowDataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsanaFlowWebServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class YogaPoseController : ControllerBase
    {
        private AsanaFlowRepository _repository;

        public YogaPoseController()
        {
            _repository = new AsanaFlowRepository();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllYogaPosesAsync()
        {
            try
            {
                var yogapose = await _repository.GetAllYogaPosesAsync();
                if (yogapose == null || yogapose.Count == 0)
                {
                    return NotFound("No yoga poses found.");
                }
                return Ok(yogapose);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("id")]
        public async Task<IActionResult> GetYogaPoseByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid yoga pose ID.");

            try
            {
                var yogapose = await _repository.GetYogaPoseByIdAsync(id);
                if (yogapose == null)
                    return NotFound($"Yoga pose with ID {id} not found.");

                return Ok(yogapose);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetYogaPosesByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Yoga pose name cannot be empty.");

            try
            {
                var yogaposes = await _repository.GetYogaPoseByNameAsync(name);
                if (yogaposes == null)
                    return NotFound($"No yoga poses found with name containing '{name}'.");

                return Ok(yogaposes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetYogaPosesByCategoryAsync(int categoryId)
        {
            if (categoryId <= 0)
                return BadRequest("Invalid category ID.");

            try
            {
                var yogaposes = await _repository.GetYogaPosesByCategoryAsync(categoryId);
                if (yogaposes == null || yogaposes.Count == 0)
                    return NotFound($"No yoga poses found for category ID {categoryId}.");

                return Ok(yogaposes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{categoryName}")]
        public async Task<IActionResult> GetYogaPosesByCategoryNameAsync(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return BadRequest("Category name cannot be empty.");

            try
            {
                var yogaposes = await _repository.GetYogaPosesByCategoryNameAsync(categoryName);
                if (yogaposes == null || yogaposes.Count == 0)
                    return NotFound($"No yoga poses found for category '{categoryName}'.");

                return Ok(yogaposes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddYogaPoseAsync([FromBody] Models.YogaPose yogapose)
        {
            if (yogapose == null)
                return BadRequest("Yoga pose data is null.");

            if (!ModelState.IsValid)
                return BadRequest("Invalid yoga pose data.");

            try
            {
                YogaPose yogaPoseObj = new YogaPose();
                yogaPoseObj.PoseId = yogapose.PoseId;
                yogaPoseObj.PoseName = yogapose.PoseName;
                yogaPoseObj.CategoryId = yogapose.CategoryId;
                yogaPoseObj.ImageUrl = yogapose.ImageUrl;
                yogaPoseObj.Instructions = yogapose.Instructions;
                yogaPoseObj.Benefits = yogapose.Benefits;
                yogaPoseObj.Precautions = yogapose.Precautions;
                yogaPoseObj.DefaultTime = yogapose.DefaultTime;

                var createdYogaPose = await _repository.AddYogaPose(yogaPoseObj);
                if (createdYogaPose == false)
                {
                    return BadRequest("Failed to add yoga pose.");
                }

                return Ok("Yoga Pose Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize (Roles = "Admin")]
        [HttpPut("UpdateNameById")]
        public async Task<IActionResult> UpdateYogaPoseNameAsync(int id, string name)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(name))
                return BadRequest("Invalid pose ID or name.");

            try
            {
                var updatePoseName = await _repository.UpdateYogaPosesNameByAsync(id, name);

                if (updatePoseName == false) return BadRequest("Yoga Pose Name Not Updated");

                return Ok("Yoga Pose Name Updated Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateBenefitById")]
        public async Task<IActionResult> UpdateYogaPosesByBenefitAsync(int id, string benefit)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(benefit))
                return BadRequest("Invalid pose ID or benefit.");
            try
            {
                var updatePoseBenefits = await _repository.UpdateYogaPosesByBenefitAsync(id, benefit);

                if (updatePoseBenefits == false) return BadRequest("Yoga Pose Not Updated");

                return Ok("Yoga Pose Benefits Updated Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateInstructionsByNId")]
        public async Task<IActionResult> UpdateYogaPoseInstructionsAsync(int id, string instructions)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(instructions))
                return BadRequest("Invalid pose ID or instructions.");

            try
            {
                var updatePoseInstructions = await _repository.UpdateYogaPoseInstructionsAsync(id, instructions);

                if (updatePoseInstructions == false) return BadRequest("Yoga Pose Not Updated");

                return Ok("Yoga Pose Instructions Updated Successfully");


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdatePrecautionsById")]
        public async Task<IActionResult> UpdateYogaPosePrecautionsAsync(int id, string precautions)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(precautions))
                return BadRequest("Invalid pose ID or precautions.");

            try
            {
                var updatePosePrecautions = await _repository.UpdateYogaPosePrecautionsAsync(id, precautions);
                if (updatePosePrecautions == false) return BadRequest("Yoga Pose Not Updated");

                return Ok("Yoga Pose Precautions Updated Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteYogaPoseAsync(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid yoga pose ID.");

            try
            {
                var deletePose = await _repository.DeleteYogaPoseAsync(id);
                if (deletePose == -1 || deletePose == -99) {
                    return NotFound($"Yoga pose with ID {id} not found or could not be deleted.");
                }
                return Ok("Yoga Pose Deleted Successfully");
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}