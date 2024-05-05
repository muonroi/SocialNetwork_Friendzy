public class CustomUnixDateTimeConverter : UnixDateTimeConverter
{
    public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (value is DateTime datetime)
        {
            if (datetime.Kind is DateTimeKind.Utc or DateTimeKind.Local)
            {
                writer.WriteValue(value);
            }
            else
            {
                base.WriteJson(writer, value, serializer);
            }
        }
        else
        {
            base.WriteJson(writer, value, serializer);
        }
    }
}