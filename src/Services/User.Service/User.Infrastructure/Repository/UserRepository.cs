namespace User.Infrastructure.Repository;

public class UserRepository(IMapper mapper, UserDbContext dbContext, IUnitOfWork<UserDbContext> unitOfWork, ILogger logger, IDapper dapper, IWorkContextAccessor workContextAccessor, ISerializeService serializeService) : RepositoryBaseAsync<UserEntity, long, UserDbContext>(dbContext, unitOfWork, workContextAccessor), IUserRepository
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private readonly ILogger _logger = logger;

    private readonly IDapper _dapper = dapper;

    private readonly ISerializeService _serializeService = serializeService;

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
        };
        UserEntity? rawResult = await _dapper.QueryFirstOrDefaultAsync<UserEntity>(command.Build(cancellationToken));
        if (rawResult is null)
        {
            return null;
        }
        _logger.Information($"END: GetUserByInput RESULT --> {_serializeService.Serialize(rawResult)} <-- ");
        UserDto result = _mapper.Map<UserDto>(rawResult);
        result.AvatarUrl = rawResult.AvatarUrl.Base64ToString();
        result.ProfileImages = rawResult.ProfileImagesUrl.Base64ToString().Replace(" ", string.Empty).Split(",") ?? [];
        result.CategoryIds = rawResult.CategoryId.Replace(" ", string.Empty).Split(",") ?? [];
        return result;
    }

    public async Task<IEnumerable<UserDto>?> GetUsersByInput(string input, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetUsersByInput --> {input} <-- ");
        DapperCommand command = new()
        {
            CommandText = CustomQuery.GetUsersByInput,
            Parameters = new
            {
                input,
                pageIndex,
                pageSize
            },
        };
        IEnumerable<UserEntity>? rawResult = await _dapper.QueryAsync<UserEntity>(command.Build(cancellationToken));
        if (rawResult is null)
        {
            return null;
        }
        _logger.Information($"END: GetUsersByInput RESULT --> {_serializeService.Serialize(rawResult)} <-- ");
        IEnumerable<UserDto> result = _mapper.Map<IEnumerable<UserDto>>(rawResult);
        result = result.Join(rawResult, user => user.Id, raw => raw.Id, (user, raw) =>
        {
            user.AvatarUrl = user.AvatarUrl.Base64ToString();
            user.ProfileImages = raw.ProfileImagesUrl.Base64ToString().Replace(" ", string.Empty).Split(",") ?? [];
            user.CategoryIds = raw.CategoryId.Replace(" ", string.Empty).Split(",") ?? [];
            return user;
        });
        return result;
    }

    public async Task<UserDto?> CreateUserByPhone(UserDto user, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: CreateUserByPhone --> {_serializeService.Serialize(user)} <-- ");
        UserEntity entity = new()
        {
            CategoryId = string.Join(',', user.CategoryIds),
            ProfileImagesUrl = string.Join(",", user.ProfileImages).StringToBase64(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            EmailAddress = user.EmailAddress,
            Address = user.Address,
            AvatarUrl = user.AvatarUrl.StringToBase64(),
            Latitude = user.Latitude,
            Longitude = user.Longitude,
            Gender = user.Gender,
            Birthdate = user.Birthdate ?? 0,
        };
        _ = await CreateAsync(entity, cancellationToken);
        _ = await SaveChangesAsync();
        _logger.Information($"END: CreateUserByPhone RESULT --> {_serializeService.Serialize(entity)} <-- ");
        return _mapper.Map<UserDto>(entity);
    }

    public async Task<bool> UpdateUserByPhone(UserDto user, string input, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: UpdateUserByPhone --> {_serializeService.Serialize(user)} <-- ");
        DapperCommand command = new()
        {
            CommandText = CustomQuery.GetUserByInput,
            Parameters = new
            {
                input
            }
        };

        UserEntity? entity = await _dapper.QueryFirstOrDefaultAsync<UserEntity>(command.Build(cancellationToken));

        entity.AccountGuid = user.AccountGuid;
        entity.CategoryId = string.Join(",", user.CategoryIds);
        entity.ProfileImagesUrl = string.Join(",", user.ProfileImages).StringToBase64();
        entity.FirstName = user.FirstName;
        entity.LastName = user.LastName;
        entity.PhoneNumber = user.PhoneNumber;
        entity.EmailAddress = user.EmailAddress;
        entity.Address = user.Address;
        entity.AvatarUrl = user.AvatarUrl.StringToBase64();
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