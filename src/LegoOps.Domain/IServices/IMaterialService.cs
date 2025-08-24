
using LegoOps.Domain;

namespace LegoOps.Application;

public interface IMaterialService
{
    Task<UnitOverviewDto?> GetMaterialStatusByUnit(string unitId);
}


