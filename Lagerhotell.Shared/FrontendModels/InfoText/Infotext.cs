namespace LagerhotellAPI.Models.DomainModels;

public class InfoText
{
    public string Text { get; set; }
    public InfoTextType Type { get; set; } = 0;
    public StorageUnitSizesGroup? StorageUnitSizeGroup { get; set; }
}
