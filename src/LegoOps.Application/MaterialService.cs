
using LegoOps.Domain;
using LegoOps.Domain.Enum;
using System;

namespace LegoOps.Application;

public class MaterialService : IMaterialService
{
    private readonly IUnitProvider _units;
    private readonly IMaterialProvider _materials;

    public MaterialService(IUnitProvider units, IMaterialProvider materials)
    {
        _units = units;
        _materials = materials;
    }

    public async Task<UnitOverviewDto?> GetMaterialStatusByUnit(string unitId)
    {
        var id = new UnitId(unitId);
        var units = await _units.GetUnitAsync(id);
        var mats = await _materials.GetMaterialsAsync(id);

        if (units is null && (mats is null || mats.Count == 0))
            return null;

        var materialDtos = mats.Select(m => new MaterialShortageDto
        {
            MaterialId = m.MaterialId.Value,
            Description = m.Description,
            RequiredQty = m.RequiredQty,
            AvailableQty = m.AvailableQty,
            UoM = m.UoM
        }).ToList();

        var shortages = materialDtos.Where(m => m.ShortageQty > 0).ToList();

        return new UnitOverviewDto
        {
            UnitId = id.Value,
            UnitName = units?.UnitName ?? "Unknown",
            ProductionStatus = units?.Status.ToString() ?? UnitStatus.Unknown.ToString(),
            LastUpdatedUtc = units?.LastUpdatedUtc,
            //Materials = materialDtos,
            Shortages = shortages
        };
    }

    public async Task<List<UnitOverviewDto>> GetAllUnitsAndMaterials()
    {        
        var result = new List<UnitOverviewDto>();
        var unit = await _units.GetAllUnitAsync();
        var mats = await _materials.GetAllMaterialsAsync();

        if (unit is null)
            return null;

        //var materialDtos = mats.Where(m => m.UnitId == m.UnitId).Select(m => new MaterialShortageDto
        //{
        //    MaterialId = m.MaterialId.Value,
        //    Description = m.Description,
        //    RequiredQty = m.RequiredQty,
        //    AvailableQty = m.AvailableQty,
        //    UoM = m.UoM
        //}).ToList();
         
        result = unit.Select(m => new UnitOverviewDto
        {
            UnitId = m.UnitId.Value,
            UnitName = m.UnitName,
            ProductionStatus = m.Status.ToString(),
            LastUpdatedUtc = m.LastUpdatedUtc
            //Shortages = materialDtos.Where(m => m.UnitId == m.UnitId && m.ShortageQty > 0).ToList()
        }).ToList();

        foreach (var u in result)
        {
            var materialDtos = mats.Where(m => m.UnitId.ToString() == u.UnitId).Select(m => new MaterialShortageDto
            {
                MaterialId = m.MaterialId.Value,
                Description = m.Description,
                RequiredQty = m.RequiredQty,
                AvailableQty = m.AvailableQty,
                UoM = m.UoM
            }).ToList();
            var shortage = materialDtos.Where(m => m.ShortageQty > 0).ToList();
            u.Shortages = shortage;
        }

        return result;
    }
}
