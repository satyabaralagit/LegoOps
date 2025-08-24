
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
    private readonly Mock<IProgressProvider> progress;
    private readonly Mock<IMaterialProvider> materials;
    public void Setup()
    {
         //progress = new Mock<IProgressProvider>();
         //materials = new Mock<IMaterialProvider>();

        progress.Setup(p => p.GetProgressAsync(It.IsAny<UnitId>()))
                .ReturnsAsync((UnitProgress?)null);
        materials.Setup(m => m.GetMaterialsAsync(It.IsAny<UnitId>()))
                 .ReturnsAsync(new List<UnitMaterial>());

        progress.Setup(p => p.GetProgressAsync(It.IsAny<UnitId>()))
                .ReturnsAsync(new UnitProgress { UnitId = new UnitId("DU-1001"), Status = UnitStatus.InProgress, LastUpdatedUtc = DateTime.UtcNow });

        materials.Setup(m => m.GetMaterialsAsync(It.IsAny<UnitId>()))
                 .ReturnsAsync(new List<UnitMaterial> {
                    new UnitMaterial { UnitId = new UnitId("DU-1001"), MaterialId = new MaterialId("MAT-001"), Description="Plywood", RequiredQty=10, AvailableQty=8, UoM="EA"},
                    new UnitMaterial { UnitId = new UnitId("DU-1001"), MaterialId = new MaterialId("MAT-002"), Description="Bracket", RequiredQty=5, AvailableQty=5, UoM="EA"}
                 });
    }

    [Fact]
    public async Task Returns_null_when_no_data_found()
    {      

        var _materialService = new MaterialService(progress.Object, materials.Object);
        var result = await _materialService.GetMaterialStatusByUnit("DU-404");
        result.Should().BeNull();
       
    }

    [Fact]
    public async Task Aggregates_materials_and_shortages()
    {
        //var progress = new Mock<IProgressProvider>();
        //var materials = new Mock<IMaterialProvider>();
        

        var _materialService = new MaterialService(progress.Object, materials.Object);
        var result = await _materialService.GetMaterialStatusByUnit("DU-1001");

        result.Should().NotBeNull();
        result!.UnitId.Should().Be("DU-1001");
        result.ProductionStatus.Should().Be(UnitStatus.InProgress.ToString());
        //result.Materials.Should().HaveCount(2);
        result.Shortages.Should().ContainSingle(s => s.MaterialId == "MAT-001" && s.ShortageQty == 2);
    }
}
