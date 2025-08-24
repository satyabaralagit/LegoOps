
using LegoOps.Domain;
using LegoOps.Domain.Enum;

namespace LegoOps.Application;

public class MaterialService : IMaterialService
{
    private readonly IProgressProvider _progress;
    private readonly IMaterialProvider _materials;

    public MaterialService(IProgressProvider progress, IMaterialProvider materials)
    {
        _progress = progress;
        _materials = materials;
    }

    public async Task<UnitOverviewDto?> GetMaterialStatusByUnit(string unitId)
    {
        var id = new UnitId(unitId);
        var progress = await _progress.GetProgressAsync(id);
        var mats = await _materials.GetMaterialsAsync(id);

        if (progress is null && (mats is null || mats.Count == 0))
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
            ProductionStatus = progress?.Status.ToString() ?? UnitStatus.Unknown.ToString(),
            LastUpdatedUtc = progress?.LastUpdatedUtc,
            //Materials = materialDtos,
            Shortages = shortages
        };
    }
}
