using LegoOps.Application;
using Microsoft.AspNetCore.Mvc;

namespace LegoOps.WebApi.Controllers;

[ApiController]
[Route("api/MaterialPlanner")]
public class MaterialPlannerController : BaseApiController
{
    private readonly IMaterialService _materialService;

    public MaterialPlannerController(IMaterialService materialService)
    {
        _materialService = materialService;
    }

    [HttpGet("MaterialShortageOverview/{unitId}")]
    public async Task<IActionResult> Get(string unitId)
    {
        try
        {
            var dto = await _materialService.GetMaterialStatusByUnit(unitId);
            if (dto is null) return BadRequestResponse("please provide correct UnitId");
            return OkResponse(dto);
        }        
        catch (Exception ex)
        {
            return BadRequestResponse("something went wrong");
        }           
        
    }

    [HttpGet("AllUnitsMaterialOverview")]
    public async Task<IActionResult> GetAllUnitsMaterialOverview()
    {
        try
        {
            var dto = await _materialService.GetAllUnitsAndMaterials();           
            return OkResponse(dto);
        }
        catch (Exception ex)
        {
            return BadRequestResponse("something went wrong");
        }

    }
}
