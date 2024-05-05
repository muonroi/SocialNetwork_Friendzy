namespace Infrastructure.Commons
{
    public class CustomUnixDateTimeConverter : DateTimeConverterBase
    {
        public override object? ReadJson(JsonReader reader, System.Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                if (reader.TokenType == JsonToken.Date)
                {
                    return reader.Value;
                }
            }
            long ticks = (long)reader.Value!;
            return DateTimeOffset.FromUnixTimeSeconds(ticks).DateTime;
        }

        public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}