
using LegoOps.Application;
using LegoOps.Domain;
using LegoOps.Domain.Enum;
using Microsoft.Extensions.DependencyInjection;

namespace LegoOps.Infrastructure.MockSap;

public class MockProgressProvider : IProgressProvider
{
    private static readonly List<UnitProgress> Data = new()
    {
        new UnitProgress { UnitId = new UnitId("DU-1001"), Status = UnitStatus.NotStarted, LastUpdatedUtc = DateTime.UtcNow.AddDays(-3) },
        new UnitProgress { UnitId = new UnitId("DU-1002"), Status = UnitStatus.InProgress, LastUpdatedUtc = DateTime.UtcNow.AddDays(-1) },
        new UnitProgress { UnitId = new UnitId("DU-1003"), Status = UnitStatus.Completed, LastUpdatedUtc = DateTime.UtcNow.AddHours(-5) }
    };

    public Task<UnitProgress?> GetProgressAsync(UnitId unitId)
    {
        if (unitId == null)
            return  Task.FromResult(Data.FirstOrDefault());
        return Task.FromResult(Data.FirstOrDefault(x => x.UnitId.Value == unitId.Value));
    }
}

public class MockMaterialProvider : IMaterialProvider
{
    private static readonly List<UnitMaterial> Data = new()
    {
        new UnitMaterial { UnitId = new UnitId("DU-1001"), MaterialId = new MaterialId("MAT-001"), Description = "Plywood Sheet", RequiredQty = 10, AvailableQty = 8, UoM = "EA" },
        new UnitMaterial { UnitId = new UnitId("DU-1001"), MaterialId = new MaterialId("MAT-002"), Description = "Metal Bracket", RequiredQty = 25, AvailableQty = 20, UoM = "EA" },
        new UnitMaterial { UnitId = new UnitId("DU-1002"), MaterialId = new MaterialId("MAT-003"), Description = "LED Strip", RequiredQty = 5, AvailableQty = 2, UoM = "EA" },
        new UnitMaterial { UnitId = new UnitId("DU-1003"), MaterialId = new MaterialId("MAT-004"), Description = "Screws", RequiredQty = 100, AvailableQty = 80, UoM = "EA" }
    };

    public Task<IReadOnlyList<UnitMaterial>> GetMaterialsAsync(UnitId unitId)
    {
        if (unitId == null)
            return Task.FromResult<IReadOnlyList<UnitMaterial>>(Data);
        return Task.FromResult<IReadOnlyList<UnitMaterial>>(Data.Where(x => x.UnitId.Value == unitId.Value).ToList());
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMockSapProviders(this IServiceCollection services)
    {
        services.AddSingleton<IProgressProvider, MockProgressProvider>();
        services.AddSingleton<IMaterialProvider, MockMaterialProvider>();
        return services;
    }
}
