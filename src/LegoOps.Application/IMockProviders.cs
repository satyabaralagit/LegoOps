
using LegoOps.Domain;

namespace LegoOps.Application;

public interface IUnitProvider
{
    Task<Units?> GetUnitAsync(UnitId unitId);
    Task<List<Units>> GetAllUnitAsync();
}

public interface IMaterialProvider
{
    Task<List<UnitMaterial>> GetMaterialsAsync(UnitId unitId);
    Task<List<UnitMaterial>> GetAllMaterialsAsync();
}
