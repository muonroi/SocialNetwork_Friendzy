using Contracts.Commons.Interfaces;
using Dapper;
using Dapper.Extensions;
using Infrastructure.Commons;
using Management.Photo.Application.Commons.Interfaces;
using Management.Photo.Application.Commons.Models;
using Management.Photo.Application.Commons.Requests;
using Management.Photo.Domain.Entities;
using Management.Photo.Infrastructure.Persistence;
using Management.Photo.Infrastructure.Persistence.Query;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Shared.Enums;
using System.Data;

namespace Management.Photo.Infrastructure.Query;

public class StoreInfoRepository(StoreInfoDbContext dbContext, IDbContextFactory<StoreInfoDbContext> dbContextFactory, IUnitOfWork<StoreInfoDbContext> unitOfWork, ILogger logger, IDapper dapper, IBucketRepository bucketRepository) : RepositoryBaseAsync<StoreInfoEntity, long, StoreInfoDbContext>(dbContext, unitOfWork), IStoreInfoRepository
{
    private readonly IDbContextFactory<StoreInfoDbContext> _dbContextFactory = dbContextFactory;

    private readonly ILogger _logger = logger;

    private readonly IDapper _dapper = dapper;

    private readonly IBucketRepository _bucketRepository = bucketRepository;

    public async Task<bool> CreateResourceAsync(CreateResourceRequest request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: CreateResourceAsync REQUEST --> {JsonConvert.SerializeObject(request)} <-- REQUEST");
        StoreInfoDbContext? storeInfoDbContext = request.IsMultiple ? _dbContextFactory.CreateDbContext() : null;
        BucketDto? bucketInfo = await _bucketRepository.GetBucketByIdAsync(request.BucketId, cancellationToken);
        if (bucketInfo is null)
        {
            _logger.Information($"END: CreateResourceAsync RESULT --> {JsonConvert.SerializeObject(false)} <-- ");
            return false;
        }
        if (storeInfoDbContext is not null)
        {
            _ = await storeInfoDbContext.StoreInfoEntities.AddAsync(new StoreInfoEntity
            {
                StoreName = request.ObjectName,
                StoreDescription = $"{request.UserId}_{request.ObjectName}_{bucketInfo.BucketName}",
                StoreUrl = request.ObjectUrl,
                UserId = request.UserId,
                BucketId = bucketInfo.Id,
                StoreInfoType = request.Type
            }, cancellationToken);
            _ = await storeInfoDbContext.SaveChangesAsync(cancellationToken);
            return true; // edit after
        }
        _ = await CreateAsync(new StoreInfoEntity
        {
            StoreName = request.ObjectName,
            StoreDescription = $"{request.UserId}_{request.ObjectName}_{bucketInfo.BucketName}",
            StoreUrl = request.ObjectUrl,
            UserId = request.UserId,
            BucketId = bucketInfo.Id,
            StoreInfoType = request.Type
        }, cancellationToken);

        long result = await SaveChangesAsync();

        _logger.Information($"END: CreateResourceAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");

        return result > 0;
    }

    public async Task<StoreInfoDTO?> GetResourceByIdAsync(long userId, long bucketId, long storyInfoId, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetResourceById REQUEST --> {JsonConvert.SerializeObject(new
        {
            userId,
            bucketId,
            storyInfoId
        })} <--");
        CommandDefinition command = new(CustomSqlQuery.GetImageById, new
        {
            userId,
            bucketId,
            storyInfoId
        }, commandType: CommandType.Text);

        StoreInfoDTO? result = await _dapper.QueryFirstOrDefaultAsync<StoreInfoDTO>(command);
        if (result is null)
        {
            _logger.Information($"END: GetResourceById RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
            return null;
        }
        _logger.Information($"END: GetResourceById RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
        return result;
    }

    public async Task<IEnumerable<StoreInfoDTO>> GetResourceByTypeAsync(long userId, long bucketId, StoreInfoType type, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetResourceByType REQUEST --> {JsonConvert.SerializeObject(new
        {
            userId,
            bucketId,
            typpe = nameof(type)
        })} <--");
        CommandDefinition command = new(CustomSqlQuery.GetImageByType, new
        {
            userId,
            bucketId,
            storeInfoType = type
        }, commandType: CommandType.Text);

        List<StoreInfoDTO> result = await _dapper.QueryAsync<StoreInfoDTO>(command);
        if (result.Count == 0)
        {
            _logger.Information($"END: GetResourceByType RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
            return [];
        }
        _logger.Information($"END: GetResourceByType RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
        return result;
    }
}