namespace LagerhotellAPI.Models.DomainModels.Validators;

public class ImageAssetValidator : AbstractValidator<ImageAsset>
{
    public ImageAssetValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Navn er obligatorisk");
        RuleFor(x => x.Name).MaximumLength(500).WithMessage("Navn kan ikke være lengre enn 500 bokstaver");
        RuleFor(x => x.Tags).NotNull().WithMessage("Tags er obligatorisk");
        RuleFor(x => x.ImageBytes).NotNull().WithMessage("Bilde er obligatorisk");
        RuleFor(x => x.ImageBytes).Must(x => x.Length < 5000000).WithMessage("Bilde kan ikke være større enn 5MB");

    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<ImageAsset>.CreateWithOptions((ImageAsset)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
