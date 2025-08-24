
using LegoOps.Domain;

namespace LegoOps.Application;

public interface IProgressProvider
{
    Task<UnitProgress?> GetProgressAsync(UnitId unitId);
}

public interface IMaterialProvider
{
    Task<IReadOnlyList<UnitMaterial>> GetMaterialsAsync(UnitId unitId);
}
