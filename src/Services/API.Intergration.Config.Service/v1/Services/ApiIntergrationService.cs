namespace API.Intergration.Config.Service.v1.Services;

public class ApiIntergrationService(IDapper dapper,
IWorkContextAccessor workContextAccessor, ILogger logger) : ApiConfigGrpcBase
{
    private readonly ILogger _logger = logger;

    public override async Task<ApiIntConfigReply> GetApiIntConfig(ApiIntConfigRequest request, ServerCallContext context)
    {
        _ = workContextAccessor.WorkContext;
        _logger.Information($"BEGIN: GetApiIntConfig REQUEST --> {JsonSerializer.Serialize(request)} <--");
        DapperCommand command = new()
        {
            CommandText = CustomSqlQuery.GetUserIntConfig,
            Parameters = new
            {
                userID = 1, //change after
                partnercode = request.PartnerCode,
                partnertype = request.PartnerType
            }
        };
        ApiIntConfigDTO result = await dapper.QueryFirstOrDefaultAsync<ApiIntConfigDTO>(
                        command.Build(context.CancellationToken),
                        enableCache: true);

        _logger.Information($"END: GetApiIntConfig RESULT --> {JsonSerializer.Serialize(result)} <--");
        return MappingApiIntergrationIntConfig(result);
    }

    private static ApiIntConfigReply MappingApiIntergrationIntConfig(ApiIntConfigDTO result)
    {
        if (result is null)
        {
            return new ApiIntConfigReply();
        }
        ApiIntConfigReply reply = new()
        {
            UserId = result.UserId.ToString(),
            PartnerCode = result.PartnerCode ?? string.Empty,
            PartnerType = result.PartnerType ?? string.Empty,
        };
        List<MethodReply>? featureGroups = JsonSerializer.Deserialize<List<MethodReply>>(result.MethodGroup);
        reply.Methods.AddRange(featureGroups);

        return reply;
    }
}