namespace User.Application.Feature.v1.Users.Queries.GetMultipleUsersQuery;

public class GetMultipleUsersQueryValidator : AbstractValidator<GetMultipleUsersQuery>
{
    public GetMultipleUsersQueryValidator()
    {
        _ = RuleFor(x => x.Input)
         .NotEmpty().WithMessage("Input is required.")
         .Must(BeValidInput)
         .WithMessage("Input must be a comma-separated list of user IDs.");
    }

    private bool BeValidInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        string[] multipleId = input.Split(',');

        return multipleId.Length > 1;
    }
}