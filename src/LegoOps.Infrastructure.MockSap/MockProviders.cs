
using LegoOps.Application;
using LegoOps.Domain;
using LegoOps.Domain.Enum;
using Microsoft.Extensions.DependencyInjection;

namespace LegoOps.Infrastructure.MockSap;

public class MockUnitProvider : IUnitProvider
{
    private static readonly List<Units> Data = new()
    {
        new Units { UnitId = new UnitId("DU-1001"), UnitName = "Unit A", Status = UnitStatus.NotStarted, LastUpdatedUtc = DateTime.UtcNow.AddDays(-3) },
        new Units { UnitId = new UnitId("DU-1002"), UnitName = "Unit B", Status = UnitStatus.InProgress, LastUpdatedUtc = DateTime.UtcNow.AddDays(-1) },
        new Units { UnitId = new UnitId("DU-1003"), UnitName = "Unit C", Status = UnitStatus.Completed, LastUpdatedUtc = DateTime.UtcNow.AddHours(-5) }
    };

    public Task<Units?> GetUnitAsync(UnitId unitId)
    {
        if (unitId == null)
            return  Task.FromResult(Data.FirstOrDefault());
        return Task.FromResult(Data.FirstOrDefault(x => x.UnitId.Value == unitId.Value));
    }
    public Task<List<Units>> GetAllUnitAsync()
    {        
        return Task.FromResult<List<Units>>(Data);
    }
}

public class MockMaterialProvider : IMaterialProvider
{
    private static readonly List<UnitMaterial> Data = new()
    {
        new UnitMaterial { UnitId = new UnitId("DU-1001"), MaterialId = new MaterialId("MAT-001"), Description = "Plywood Sheet", RequiredQty = 10, AvailableQty = 8, UoM = "#No" },
        new UnitMaterial { UnitId = new UnitId("DU-1001"), MaterialId = new MaterialId("MAT-002"), Description = "Metal Bracket", RequiredQty = 25, AvailableQty = 20, UoM = "#No" },
        new UnitMaterial { UnitId = new UnitId("DU-1002"), MaterialId = new MaterialId("MAT-003"), Description = "LED Strip", RequiredQty = 5, AvailableQty = 2, UoM = "#No" },
        new UnitMaterial { UnitId = new UnitId("DU-1002"), MaterialId = new MaterialId("MAT-004"), Description = "Chip", RequiredQty = 200, AvailableQty = 150, UoM = "#No" },
        new UnitMaterial { UnitId = new UnitId("DU-1003"), MaterialId = new MaterialId("MAT-005"), Description = "Screws", RequiredQty = 100, AvailableQty = 80, UoM = "#No" }
    };

    public Task<List<UnitMaterial>> GetMaterialsAsync(UnitId unitId)
    {
        if (unitId == null)
            return Task.FromResult<List<UnitMaterial>>(Data);
        return Task.FromResult<List<UnitMaterial>>(Data.Where(x => x.UnitId.Value == unitId.Value).ToList());
    }

    public Task<List<UnitMaterial>> GetAllMaterialsAsync()
    {        
            return Task.FromResult<List<UnitMaterial>>(Data);
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMockSapProviders(this IServiceCollection services)
    {
        services.AddSingleton<IUnitProvider, MockUnitProvider>();
        services.AddSingleton<IMaterialProvider, MockMaterialProvider>();
        return services;
    }
}
