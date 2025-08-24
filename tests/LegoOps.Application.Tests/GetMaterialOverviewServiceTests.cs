
using Xunit;
using Moq;
using FluentAssertions;
using LegoOps.Application;
using LegoOps.Domain;
using System.Threading;
using LegoOps.Domain.Enum;

namespace LegoOps.Application.Tests;

public class GetMaterialOverviewServiceTests
{
    private readonly Mock<IUnitProvider> progress;
    private readonly Mock<IMaterialProvider> materials;
    public void Setup()
    {
         //progress = new Mock<IProgressProvider>();
         //materials = new Mock<IMaterialProvider>();

        progress.Setup(p => p.GetUnitAsync(It.IsAny<UnitId>()))
                .ReturnsAsync((Units?)null);
        materials.Setup(m => m.GetMaterialsAsync(It.IsAny<UnitId>()))
                 .ReturnsAsync(new List<UnitMaterial>());
        
    }

    [Fact]
    public async Task Returns_null_when_no_data_found()
    {
        Setup();
        var _materialService = new MaterialService(progress.Object, materials.Object);
        var result = await _materialService.GetMaterialStatusByUnit("DU-404");
        result.Should().BeNull();
       
    }

    [Fact]
    public async Task Aggregates_materials_and_shortages()
    {
        var unit = new Mock<IUnitProvider>();
        var materials = new Mock<IMaterialProvider>();

        unit.Setup(p => p.GetUnitAsync(It.IsAny<UnitId>()))
                .ReturnsAsync(new Units { UnitId = new UnitId("DU-1001"), UnitName = "Unit A", Status = UnitStatus.InProgress, LastUpdatedUtc = DateTime.UtcNow });

        materials.Setup(m => m.GetMaterialsAsync(It.IsAny<UnitId>()))
                 .ReturnsAsync(new List<UnitMaterial> {
                    new UnitMaterial { UnitId = new UnitId("DU-1001"), MaterialId = new MaterialId("MAT-001"), Description="Plywood", RequiredQty=10, AvailableQty=8, UoM="EA"},
                    new UnitMaterial { UnitId = new UnitId("DU-1001"), MaterialId = new MaterialId("MAT-002"), Description="Bracket", RequiredQty=5, AvailableQty=5, UoM="EA"}
                 });

        var _materialService = new MaterialService(unit.Object, materials.Object);
        var result = await _materialService.GetMaterialStatusByUnit("DU-1001");

        result.Should().NotBeNull();
        result!.UnitId.Should().Be("DU-1001");
        result.ProductionStatus.Should().Be(UnitStatus.InProgress.ToString());
        //result.Materials.Should().HaveCount(2);
        result.Shortages.Should().ContainSingle(s => s.MaterialId == "MAT-001" && s.ShortageQty == 2);
    }
}
