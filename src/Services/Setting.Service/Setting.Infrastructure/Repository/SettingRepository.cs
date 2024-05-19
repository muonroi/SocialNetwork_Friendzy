namespace Setting.Infrastructure.Repository;

public class SettingRepository<T, TK>(SettingDbContext dbContext, IUnitOfWork<SettingDbContext> unitOfWork, ILogger logger) : RepositoryBaseAsync<T, TK, SettingDbContext>(dbContext, unitOfWork), ISettingRepository<T, TK> where T : EntityBase<TK>
{
    private readonly ILogger _logger = logger;

    public async Task<T?> GetSettingByType(Expression<Func<T, bool>> expresion)
    {
        _logger.Information($"BEGIN: GetSettingByKey");
        T? result = await FindObjectByCondition(expresion);
        if (result is null)
        {
            return null;
        }
        _logger.Information($"END: GetSettingByKey RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
        return result;
    }
}