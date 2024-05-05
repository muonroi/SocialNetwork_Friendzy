using System.Data;
using System.Data.Common;

namespace Infrastructure.ORMs.Dappers.Interfaces
{
    public interface IDapperCustom : IDisposable
    {
        DbConnection GetDbConnection();

        Task<T?> Get<T>(string storedProcedure, object parameters, CommandType commandType = CommandType.StoredProcedure);

        Task<List<T>?> GetAll<T>(string storedProcedure, object parameters, CommandType commandType = CommandType.StoredProcedure);

        Task<(IEnumerable<T>? Data, int TotalCount)> GetAllPaging<T>(string storedProcedure, string storedProcedureCount, object parameters, object countpParameters, CommandType commandType = CommandType.StoredProcedure);

        Task<int> Execute(string storedProcedure, object parameters, CommandType commandType = CommandType.StoredProcedure);

        Task<T?> Insert<T>(string storedProcedure, object parameters, CommandType commandType = CommandType.StoredProcedure);

        Task<T?> Update<T>(string storedProcedure, object parameters, CommandType commandType = CommandType.StoredProcedure);
    }
}