using AutoMapper;
using Contracts.Commons.Interfaces;
using Dapper.Extensions;
using Infrastructure.Commons;
using Management.Photo.Application.Commons.Interfaces;
using Management.Photo.Domain.Entities;
using Management.Photo.Infrastructure.Persistences;
using Serilog;

namespace Management.Photo.Infrastructure.Repository
{
    public class StoreInfoRepository(IMapper mapper, StoreInfoDbContext dbContext, IUnitOfWork<StoreInfoDbContext> unitOfWork, ILogger logger, IDapper dapper) : RepositoryBaseAsync<StoreInfoEntity, long, StoreInfoDbContext>(dbContext, unitOfWork), IStoreInfoRepository
    {
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        private readonly ILogger _logger = logger;

        private readonly IDapper _dapper = dapper;
    }
}