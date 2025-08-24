
using LegoOps.Domain;

namespace LegoOps.Application;

public class MaterialShortageDto
{
    public string MaterialId { get; set; } 
    public string Description { get; set; }
    public decimal RequiredQty { get; set; }
    public decimal AvailableQty { get; set; }
    public decimal ShortageQty => Math.Max(0, RequiredQty - AvailableQty);
    public string UoM { get; set; }
}

public class UnitOverviewDto
{
    public string UnitId { get; set; } 
    public string ProductionStatus { get; set; } 
    public DateTime? LastUpdatedUtc { get; set; }
    public List<MaterialShortageDto> Shortages { get; set; }
    //public List<MaterialShortageDto> Materials { get; set; } 
}
