namespace API.Intergration.Config.Service.v1.Services;

public class ApiIntergrationService(IDapper dapper,
IWorkContextAccessor workContextAccessor) : ApiConfigGrpcBase
{
    public override async Task<ApiIntConfigReply> GetApiIntConfig(ApiIntConfigRequest request, ServerCallContext context)
    {
        DapperCommand command = new()
        {
            CommandText = CustomSqlQuery.GetUserIntConfig,
            Parameters = new
            {
                userID = workContextAccessor!.WorkContext!.UserId!,
                partnercode = request.PartnerCode,
                partnertype = request.PartnerType
            }
        };
        ApiIntConfigDTO result = await dapper.QueryFirstOrDefaultAsync<ApiIntConfigDTO>(
                        command.Build(context.CancellationToken),
                        enableCache: true);

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