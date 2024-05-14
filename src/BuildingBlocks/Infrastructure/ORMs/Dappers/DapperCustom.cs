namespace Infrastructure.ORMs.Dappers;

public class DapperCustom(IConfiguration configuration) : IDapperCustom
{
    private readonly IConfiguration _configuration = configuration;

    private readonly string ConnectionStringName = "DefaultConnectionString";

    public async Task<T?> Get<T>(string storedProcedure, object parameters, CommandType commandType = CommandType.Text)
    {
        using SqlConnection connection = new(_configuration.GetConnectionString(ConnectionStringName));
        return await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: commandType);
    }

    public async Task<(IEnumerable<T>? Data, int TotalCount)> GetAllPaging<T>(string storedProcedure, string storedProcedureCount, object parameters, object countpParameters, CommandType commandType = CommandType.StoredProcedure)
    {
        using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
        using SqlConnection connection = new(_configuration.GetConnectionString(ConnectionStringName));
        connection.Open();

        IEnumerable<T> result = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: commandType);

        int countResult = await connection.QuerySingleAsync<int>(storedProcedureCount, countpParameters, commandType: commandType);

        scope.Complete();

        return (result, countResult);
    }

    public async Task<List<T>?> GetAll<T>(string storedProcedure, object parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        using SqlConnection connection = new(_configuration.GetConnectionString(ConnectionStringName));
        IEnumerable<T> result = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: commandType);
        return result.ToList();
    }

    public async Task<T?> Insert<T>(string storedProcedure, object parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        using SqlConnection connection = new(_configuration.GetConnectionString(ConnectionStringName));
        await connection.OpenAsync();
        using SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            T? result = await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: commandType, transaction: transaction);
            transaction.Commit();
            return result;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public async Task<T?> Update<T>(string storedProcedure, object parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        using SqlConnection connection = new(_configuration.GetConnectionString(ConnectionStringName));
        await connection.OpenAsync();
        using SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            T? result = await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: commandType, transaction: transaction);
            transaction.Commit();
            return result;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public async Task<int> Execute(string storedProcedure, object parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        using SqlConnection connection = new(_configuration.GetConnectionString(ConnectionStringName));
        return await connection.ExecuteAsync(storedProcedure, parameters, commandType: commandType);
    }

    public DbConnection GetDbConnection()
    {
        return new SqlConnection(_configuration.GetConnectionString(ConnectionStringName));
    }

    public void Dispose()
    {
    }
}