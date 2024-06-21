namespace Setting.Infrastructure.Repository;

public class SettingRepository<T, TK>(SettingDbContext dbContext, IUnitOfWork<SettingDbContext> unitOfWork, ILogger logger, IWorkContextAccessor workContextAccessor, ISerializeService serializeService) : RepositoryBaseAsync<T, TK, SettingDbContext>(dbContext, unitOfWork, workContextAccessor), ISettingRepository<T, TK> where T : EntityAuditBase<TK>
{
    private readonly ILogger _logger = logger;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task<T?> GetSettingByType(Expression<Func<T, bool>> expresion)
    {
        _logger.Information($"BEGIN: GetSettingByType REQUEST --> Expression<Func<T, bool>> expresion <--");
        T? result = await FindObjectByCondition(expresion);
        if (result is null)
        {
            return null;
        }
        _logger.Information($"END: GetSettingByType RESULT --> {_serializeService.Serialize(result)} <-- ");
        return result;
    }

    public async Task CreateSettingByType(T request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: CreateSettingByType REQUEST --> Expression<Func<T, bool>> expresion <--");

        TK? result = await CreateAsync(request, cancellationToken);

        _logger.Information($"END: CreateSettingByType RESULT --> {_serializeService.Serialize(result)} <-- ");
    }
}