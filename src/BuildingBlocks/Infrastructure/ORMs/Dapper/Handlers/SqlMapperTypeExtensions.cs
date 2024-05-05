using Dapper;

namespace Infrastructure.ORMs.Dapper.Handlers
{
    public static class SqlMapperTypeExtensions
    {
        public static void RegisterDapperHandlers()
        {
            SqlMapper.ResetTypeHandlers();
            SqlMapper.AddTypeHandler(new ProtobufTimestampHandler());
            SqlMapper.AddTypeHandler(new StringValueHandler());
        }
    }
}