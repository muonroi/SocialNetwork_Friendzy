namespace Setting.Application.Commons.Interfaces;

public interface ISettingRepository<T, TK> : IRepositoryBaseAsync<T, TK> where T : EntityBase<TK>
{
    Task<T?> GetSettingByType(Expression<Func<T, bool>> expresion);
}