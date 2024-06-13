namespace Account.Application.Infrastructure.feature.v1.Accounts.Commands.CreateAccountCommand;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        _ = RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must be less than 50 characters.");

        _ = RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must be less than 50 characters.");

        _ = RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.");

        _ = RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Email address must be in the correct format.");

        _ = RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(1000).WithMessage("Address must be less than 100 characters.");
    }
}