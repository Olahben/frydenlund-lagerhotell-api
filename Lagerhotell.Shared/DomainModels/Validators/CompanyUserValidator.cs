namespace LagerhotellAPI.Models.DomainModels.Validators;

public class CompanyUserValidator : AbstractValidator<CompanyUser>
{
    public CompanyUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Fornavn er obligatorisk");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Etternavn er obligatorisk");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Navn er obligatorisk");
        RuleFor(x => x.CompanyNumber).NotEmpty().WithMessage("Organisasjonsnummer er obligatorisk");
        RuleFor(x => x.CompanyNumber).LessThanOrEqualTo(999999999).WithMessage("Organisasjonsnummer kan ikke være lengre enn 9 siffer");
        RuleFor(x => x.CompanyNumber).GreaterThanOrEqualTo(100000000).WithMessage("Organisasjonsnummer kan ikke være kortere enn 9 siffer");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Epost er obligatorisk");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Epost er ikke gyldig");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefonnummer er obligatorisk");
        RuleFor(x => x.PhoneNumber).Matches(@"^(\+47|0047|47)?[1-9]\d{7}$").WithMessage("Telefonnummer er ikke gyldig, husk +47");
        RuleFor(x => x.Address).NotNull().WithMessage("Adresse er obligatorisk");
        RuleFor(x => x.Address.StreetAddress).NotEmpty().WithMessage("Gate er obligatorisk");
        RuleFor(x => x.Address.PostalCode).NotEmpty().WithMessage("Postnummer er obligatorisk");
        RuleFor(x => x.Address.PostalCode).Matches(@"^\d{4}$").WithMessage("Postnummer er ikke gyldig");
        RuleFor(x => x.Address.City).NotEmpty().WithMessage("By er obligatorisk");

    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<CompanyUser>.CreateWithOptions((CompanyUser)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
