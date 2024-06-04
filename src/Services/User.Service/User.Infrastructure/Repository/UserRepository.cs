namespace User.Infrastructure.Repository;

public class UserRepository(IMapper mapper, UserDbContext dbContext, IUnitOfWork<UserDbContext> unitOfWork, ILogger logger, IDapper dapper) : RepositoryBaseAsync<UserEntity, long, UserDbContext>(dbContext, unitOfWork), IUserRepository
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private readonly ILogger _logger = logger;

    private readonly IDapper _dapper = dapper;

    public async Task<UserDto?> GetUserByInput(string input, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetUserByInput --> {input} <-- ");
        DapperCommand command = new()
        {
            CommandText = CustomQuery.GetUserByInput,
            Parameters = new
            {
                input
            },
            CommandType = CommandType.StoredProcedure
        };
        UserEntity? rawResult = await _dapper.QueryFirstOrDefaultAsync<UserEntity>(command.Build(cancellationToken));
        if (rawResult is null)
        {
            return null;
        }
        _logger.Information($"END: GetUserByInput RESULT --> {JsonConvert.SerializeObject(rawResult)} <-- ");
        UserDto result = _mapper.Map<UserDto>(rawResult);
        result.ProfileImages = rawResult.ProfileImagesUrl.Replace(" ", string.Empty).Split(",") ?? [];
        return result;
    }

    public async Task<IEnumerable<UserDto>?> GetUsersByInput(string input, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetUsersByInput --> {input} <-- ");
        DapperCommand command = new()
        {
            CommandText = CustomQuery.GetUsersByInput,
            Parameters = new
            {
                input
            },
        };
        List<UserEntity>? rawResult = await _dapper.QueryAsync<UserEntity>(command.Build(cancellationToken));
        if (rawResult is null)
        {
            return null;
        }
        _logger.Information($"END: GetUsersByInput RESULT --> {JsonConvert.SerializeObject(rawResult)} <-- ");
        IEnumerable<UserDto> result = _mapper.Map<IEnumerable<UserDto>>(rawResult);
        result = result.Join(rawResult, user => user.Id, raw => raw.Id, (user, raw) =>
        {
            user.ProfileImages = raw.ProfileImagesUrl.Replace(" ", string.Empty).Split(",") ?? [];
            return user;
        });
        return result;
    }

    public async Task<UserDto?> CreateUserByPhone(UserDto user, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: CreateUserByPhone --> {JsonConvert.SerializeObject(user)} <-- ");
        UserEntity entity = new()
        {
            CategoryId = user.CategoryId,
            ProfileImagesUrl = string.Join(",", user.ProfileImages),
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            EmailAddress = user.EmailAddress,
            Address = user.Address,
            AvatarUrl = user.AvatarUrl,
            Latitude = user.Latitude,
            Longitude = user.Longitude,
            Gender = user.Gender,
            Birthdate = user.Birthdate ?? 0,
        };
        _ = await CreateAsync(entity, cancellationToken);
        _ = await SaveChangesAsync();
        _logger.Information($"END: CreateUserByPhone RESULT --> {JsonConvert.SerializeObject(entity)} <-- ");
        return _mapper.Map<UserDto>(entity);

    }

    public async Task<bool> UpdateUserByPhone(UserDto user, string input, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: UpdateUserByPhone --> {JsonConvert.SerializeObject(user)} <-- ");
        DapperCommand command = new()
        {
            CommandText = CustomQuery.GetUserByInput,
            Parameters = new
            {
                input
            },
            CommandType = CommandType.StoredProcedure
        };
        UserEntity? entity = await _dapper.QueryFirstOrDefaultAsync<UserEntity>(command.Build(cancellationToken));
        entity.AccountGuid = user.AccountGuid;
        entity.CategoryId = user.CategoryId;
        entity.ProfileImagesUrl = string.Join(",", user.ProfileImages);
        entity.FirstName = user.FirstName;
        entity.LastName = user.LastName;
        entity.PhoneNumber = user.PhoneNumber;
        entity.EmailAddress = user.EmailAddress;
        entity.Address = user.Address;
        entity.AvatarUrl = user.AvatarUrl;
        entity.Latitude = user.Latitude;
        entity.Longitude = user.Longitude;
        entity.Gender = user.Gender;
        entity.Birthdate = user.Birthdate ?? 0;
        await UpdateAsync(entity);
        int result = await SaveChangesAsync();
        _logger.Information($"END: UpdateUserByPhone RESULT --> {result} <-- ");
        return result > 0;
    }
}