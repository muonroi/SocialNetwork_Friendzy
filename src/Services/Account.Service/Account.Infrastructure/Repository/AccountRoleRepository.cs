namespace Account.Infrastructure.Repository;

public class AccountRoleRepository(IMapper mapper, AccountDbContext dbContext, IUnitOfWork<AccountDbContext> unitOfWork, ILogger logger, IDapper dapper, IWorkContextAccessor workContextAccessor, ISerializeService serializeService) : RepositoryBaseAsync<AccountRolesEntity, Guid, AccountDbContext>(dbContext, unitOfWork, workContextAccessor), IAccountRoleRepository
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private readonly ILogger _logger = logger;

    private readonly IDapper _dapper = dapper;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task<bool> AssignAccountToRoleId(Guid accountId, Guid roleId, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information($"BEGIN: CreateAccountAsync REQUEST --> {_serializeService.Serialize(new { accountId, roleId })} <-- REQUEST");
            _ = await CreateAsync(new AccountRolesEntity
            {
                AccountId = accountId,
                RoleId = roleId
            }, cancellationToken);

            int result = await SaveChangesAsync();

            _logger.Information($"END: CreateAccountAsync RESULT --> {_serializeService.Serialize(result)} <-- ");

            return result > 0;
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            throw;
        }
    }
}