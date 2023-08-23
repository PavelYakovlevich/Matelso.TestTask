using System.Data;
using System.Text.RegularExpressions;
using FluentValidation;
using Models.ContactManager;

namespace ContactManager.API.Validators;

public class APIActionContactModelValidator : AbstractValidator<APIActionContactModel>
{
    public APIActionContactModelValidator()
    {
        RuleFor(contact => contact.Email).EmailAddress().NotEmpty().WithMessage("Email is invalid");
        RuleFor(contact => contact.PhoneNumber)
            .Matches(new Regex(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$"))
            .When(contact => contact.PhoneNumber is not null)
            .WithMessage("Phone number is invalid");

        RuleFor(contact => contact.Salutation).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(contact => contact.FirstName).MinimumLength(2).MaximumLength(50);
        RuleFor(contact => contact.LastName).MinimumLength(2).MaximumLength(50);

        RuleFor(contact => contact.Birthday).LessThan(DateTime.UtcNow)
            .When(contact => contact.Birthday is not null)
            .WithMessage("Birthday must be in the past");
    }
}