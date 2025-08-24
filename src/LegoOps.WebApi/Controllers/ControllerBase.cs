using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LegoOps.WebApi.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult OkResponse<T>(T data, string message = "Operation successful.")
        {
            return Ok(new ApiResponse<T>(true, message, data));
        }

        protected IActionResult BadRequestResponse(string message = "Bad request.", List<string> errors = null)
        {
            return BadRequest(new ApiResponse<object>(false, message, errors: errors));
        }

        protected IActionResult NotFoundResponse(string message = "Resource not found.")
        {
            return NotFound(new ApiResponse<object>(false, message));
        }

        // Add other common response types as needed (e.g., Created, Unauthorized)
    }
}
