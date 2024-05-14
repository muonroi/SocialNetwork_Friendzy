namespace Infrastructure.ORMs.Dapper.Handlers;

public class StringValueHandler : SqlMapper.TypeHandler<string>
{
    public override string? Parse(object value)
    {
        return value.ToString()?.Trim();
    }

    public override void SetValue(IDbDataParameter parameter, string? value)
    {
        throw new NotImplementedException();
    }
}