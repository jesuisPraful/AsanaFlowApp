using AsanaFlowDataAccessLayer;
using AsanaFlowDataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsanaFlowWebServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class YogaCategoryController : ControllerBase
    {
        private AsanaFlowRepository _repository;    
        public YogaCategoryController()
        {
            _repository = new AsanaFlowRepository();
        }

        [Authorize]
        [HttpGet("GetAllYogaCategories")]
        public async Task<IActionResult> GetAllYogaCategoriesAsync()
        {
            try
            {
                var YogaCategory = await _repository.GetAllYogaCategoriesAsync();
                if (YogaCategory.Count == 0 || YogaCategory == null)
                {
                    return NotFound("No yoga categories found.");
                }
                return Ok(YogaCategory);

            }
            catch (Exception ex){
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize]
        [HttpGet("{categoryName}")]
        public async Task<IActionResult> GetCategoryByNameAsync(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return BadRequest("Category name cannot be empty.");

            try
            {
                var yogaCategory = await _repository.GetCategoryByNameAsync(categoryName);
                if (yogaCategory == null)
                    return NotFound($"Yoga category with name '{categoryName}' not found.");

                return Ok(yogaCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize (Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddYogaCategoryAsync([FromBody] Models.YogaCategory yogaCategory)
        {
            if (yogaCategory == null ) return BadRequest("Category can not empty.");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                YogaCategory yogaCategory1 = new YogaCategory
                {
                    CategoryName = yogaCategory.CategoryName,
                    Description = yogaCategory.Description
                };
                
                var result = await _repository.AddYogaCategoryAsync(yogaCategory1);
                if(result ==false) return BadRequest("Failed to add category.");
                return Ok("Category added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("id")]
        public async Task<IActionResult> UpdateYogaCategoryDescriptionById(int categoryId,string description)
        {
            if (categoryId <= 0) return BadRequest("Invalid category ID.");
            if (string.IsNullOrWhiteSpace(description)) return BadRequest("Description cannot be empty.");

            try
            {
                var result = await _repository.UpdateYogaCategoryDescriptionById(categoryId, description);
                if(result ==false) return BadRequest("Failed to update category description.");
                return Ok("Category description updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("id")]

        public async Task<IActionResult> DeleteYogaCategoryAsync(int categoryId)
        {
            if(categoryId <= 0) return BadRequest("Invalid category ID.");
            try
            {
                var result = await _repository.DeleteYogaCategoryAsync(categoryId);
                if (result == false) return BadRequest("Failed to delete category.");
                return Ok("Category deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
