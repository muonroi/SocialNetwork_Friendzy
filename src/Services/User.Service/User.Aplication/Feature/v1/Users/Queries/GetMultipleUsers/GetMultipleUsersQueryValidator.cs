namespace User.Application.Feature.v1.Users.Queries.GetMultipleUsers
{
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

            if (multipleId.Length <= 1)
            {
                return false;
            }

            foreach (string id in multipleId)
            {
                if (!long.TryParse(id, out _))
                {
                    return false;
                }
            }

            return true;
        }
    }
}