namespace User.Application.Feature.v1.Users.Commands.UserLoginCommand;

public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
{
    public UserLoginCommandValidator()
    {
        _ = RuleFor(x => x.PhoneNumber)
         .NotEmpty().WithMessage("Phone number is required");
    }
}