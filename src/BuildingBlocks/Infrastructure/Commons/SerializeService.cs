﻿namespace Infrastructure.Commons;

public class SerializeService : ISerializeService
{
    public string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters =
            [
                new StringEnumConverter()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            ]
        });
    }

    public string Serialize<T>(T obj, System.Type type)
    {
        return JsonConvert.SerializeObject(obj, type, new JsonSerializerSettings());
    }

    public T? Deserialize<T>(string text)
    {
        return JsonConvert.DeserializeObject<T>(text);
    }
}