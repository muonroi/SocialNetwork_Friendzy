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
}