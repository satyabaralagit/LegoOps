
using LegoOps.Domain.Enum;

namespace LegoOps.Domain;

public class UnitId
{
    public string Value { get; }
    public UnitId(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("UnitId cannot be empty");
        Value = value;
    }
    public override string ToString() => Value;
}

public class MaterialId
{
    public string Value { get; }
    public MaterialId(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("MaterialId cannot be empty");
        Value = value;
    }
    public override string ToString() => Value;
}

public class Units
{
    public UnitId UnitId { get; init; } = new("UNKNOWN");
    public string UnitName { get; init; } = string.Empty;
    public UnitStatus Status { get; init; } = UnitStatus.Unknown;
    public DateTime LastUpdatedUtc { get; init; } = DateTime.UtcNow;
}

public class UnitMaterial
{
    public UnitId UnitId { get; init; } = new("UNKNOWN");
    public MaterialId MaterialId { get; init; } = new("UNKNOWN");
    public string Description { get; init; } = string.Empty;
    public decimal RequiredQty { get; init; }
    public decimal AvailableQty { get; init; }
    public string UoM { get; init; } = "EA";
}
