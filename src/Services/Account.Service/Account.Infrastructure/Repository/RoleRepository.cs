namespace Account.Infrastructure.Repository;

internal class RoleRepository(
    IMapper mapper,
    AccountDbContext dbContext,
    IUnitOfWork<AccountDbContext> unitOfWork,
    ILogger logger, IDapper dapper,
    IWorkContextAccessor workContextAccessor,
    ISerializeService serializeService) : RepositoryBaseAsync
                                        <AccountRolesEntity,
                                          Guid,
                                          AccountDbContext>(dbContext, unitOfWork, workContextAccessor), IRoleRepository
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private readonly ILogger _logger = logger;

    private readonly IDapper _dapper = dapper;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task<IEnumerable<RoleDTO>> GetAllRole(CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetAllRole REQUEST -->  <-- REQUEST");
        DapperCommand command = new()
        {
            CommandText = CustomQuery.GetAllRoles
        };
        IEnumerable<RoleDTO>? result = await _dapper.QueryAsync<RoleDTO>(command.Build(cancellationToken));
        if (result is null)
        {
            return [];
        }

        _logger.Information($"END: GetAllRole RESULT --> {_serializeService.Serialize(result)} <-- ");

        return result;
    }

    public async Task<RoleDTO?> GetRoleByRoleName(RoleConstants role, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetRoleByRoleName REQUEST --> {new { role = nameof(role) }} <-- REQUEST");
        DapperCommand command = new()
        {
            CommandText = CustomQuery.GetRoleByRoleName,
            Parameters = new
            {
                roleName = role.ToString()
            }
        };
        RoleDTO? result = await _dapper.QueryFirstOrDefaultAsync<RoleDTO>(command.Build(cancellationToken));
        if (result is null)
        {
            return new RoleDTO();
        }

        _logger.Information($"END: GetRoleByRoleName RESULT --> {_serializeService.Serialize(result)} <-- ");

        return result;
    }
}