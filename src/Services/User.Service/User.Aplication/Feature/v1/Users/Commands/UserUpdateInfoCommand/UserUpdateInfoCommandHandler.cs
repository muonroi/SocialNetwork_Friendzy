namespace User.Application.Feature.v1.Users.Commands.UserUpdateInfoCommand;

public class UserUpdateInfoCommandHandler(IUserRepository userRepository, IWorkContextAccessor workContextAccessor) : IRequestHandler<UserUpdateInfoCommand, ApiResult<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor ?? throw new ArgumentNullException(nameof(workContextAccessor));

    public async Task<ApiResult<UserDto>> Handle(UserUpdateInfoCommand request, CancellationToken cancellationToken)
    {
        WorkContextInfoDTO workContext = _workContextAccessor.WorkContext!;

        UserDto? userResult = await _userRepository.GetUserByInput(workContext.PhoneNumber, cancellationToken);

        if (userResult == null)
        {
            return new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound);
        }

        bool updateResult = await _userRepository.UpdateUserByPhone(new UserDto
        {
            Id = userResult.Id,
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailAddress = request.EmailAddress ?? string.Empty,
            AvatarUrl = request.AvatarUrl,
            Address = request.Address,
            ProfileImages = request.ProfileImagesUrl.Split(","),
            Longitude = request.Longitude,
            Latitude = request.Latitude,
            CategoryIds = userResult.CategoryIds,
            Birthdate = request.Birthdate,
            AccountGuid = userResult.AccountGuid,
        }, workContext.PhoneNumber, cancellationToken);

        if (!updateResult)
        {
            return new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserUpdateFailed}", (int)HttpStatusCode.BadRequest);
        }

        UserDto? result = await _userRepository.GetUserByInput(workContext.PhoneNumber, cancellationToken);

        return result is null
            ? new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<UserDto>(userResult);
    }
}