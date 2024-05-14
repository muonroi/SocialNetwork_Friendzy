using Contracts.Commons.Interfaces;
using Dapper;
using Dapper.Extensions;
using Infrastructure.Commons;
using Management.Photo.Application.Commons.Interfaces;
using Management.Photo.Application.Commons.Models;
using Management.Photo.Domain.Entities;
using Management.Photo.Infrastructure.Persistences;
using Management.Photo.Infrastructure.Persistences.Query;
using Newtonsoft.Json;
using Serilog;
using Shared.Enums;
using System.Data;

namespace Management.Photo.Infrastructure.Repository;

public class StoreInfoRepository(StoreInfoDbContext dbContext, IUnitOfWork<StoreInfoDbContext> unitOfWork, ILogger logger, IDapper dapper) : RepositoryBaseAsync<StoreInfoEntity, long, StoreInfoDbContext>(dbContext, unitOfWork), IStoreInfoRepository
{
    private readonly ILogger _logger = logger;

    private readonly IDapper _dapper = dapper;

    public async Task<List<StoreInfoDTO>> GetResourceByName(long userId, StoreInfoType type)
    {
        _logger.Information($"BEGIN: GetResourceByName");
        CommandDefinition command = new(CustomSqlQuery.GetImageByName, new
        {
            userId,
            storeInfoType = type,
        }, commandType: CommandType.Text);

        List<StoreInfoDTO> result = await _dapper.QueryAsync<StoreInfoDTO>(command);
        if (result.Count == 0)
        {
            _logger.Information($"END: GetResourceByName RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
            return [];
        }
        _logger.Information($"END: GetResourceByName RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
        return result;
    }

    Task<List<StoreInfoDTO>> IStoreInfoRepository.GetResourceByName(long userId, StoreInfoType type)
    {
        throw new NotImplementedException();
    }
}