namespace LagerhotellAPI.Models.FrontendModels.Custom;

public struct StorageUnitSize
{
    public double? Area { get; set; }
    public double? Volume { get; set; }
    public Money Price { get; set; }
    public bool Temperated { get; set; }
    public string? InfoText { get; set; }
}
