namespace Infrastructure.Extensions;

public class QueryParamSerializer : RequestQueryParamSerializer
{
    public override IEnumerable<KeyValuePair<string, string?>> SerializeQueryParam<T>(string name, T value, RequestQueryParamSerializerInfo info)
    {
        return Serialize(name, value, info);
    }

    public override IEnumerable<KeyValuePair<string, string?>> SerializeQueryCollectionParam<T>(string name, IEnumerable<T> values, RequestQueryParamSerializerInfo info)
    {
        return Serialize(name, values, info);
    }

    private IEnumerable<KeyValuePair<string, string?>> Serialize<T>(string name, T value, RequestQueryParamSerializerInfo info)
    {
        if (value is null)
        {
            yield break;
        }

        foreach (KeyValuePair<string, object?> prop in GetPropertiesDeepRecursive(value, name))
        {
            yield return prop.Value is null
                ? new KeyValuePair<string, string?>(prop.Key, string.Empty)
                : prop.Value is DateTime dt
                    ? new KeyValuePair<string, string?>(prop.Key, dt.ToString(info.Format ?? "o"))
                    : new KeyValuePair<string, string?>(prop.Key, prop.Value?.ToString());
        }
    }

    private Dictionary<string, object?> GetPropertiesDeepRecursive(object obj, string name)
    {
        Dictionary<string, object?> dict = [];

        if (obj is null)
        {
            dict.Add(name, null);
            return dict;
        }

        if (obj.GetType().IsValueType || obj is string)
        {
            dict.Add(name, obj);
            return dict;
        }

        if (obj is IEnumerable collection)
        {
            int i = 0;
            foreach (object? item in collection)
            {
                dict = dict.Concat(GetPropertiesDeepRecursive(item, $"{name}[{i++}]")).ToDictionary(e => e.Key, e => e.Value);
            }
            return dict;
        }

        PropertyInfo[] properties = obj.GetType().GetProperties();

        //If the prefix won't be empty, then it is needed to specify [Query(null)].
        //Otherwise, the query string will contain the query name e.g. 'query.page' instead of just 'page'.
        //var prefix = string.IsNullOrWhiteSpace(name) ? string.Empty : $"{name}.";
        string prefix = string.Empty;
        foreach (PropertyInfo prop in properties)
        {
            object? tmp = prop.GetValue(obj, null);
            if (tmp is null)
            {
                continue;
            }

            dict = dict
                .Concat(GetPropertiesDeepRecursive(tmp, $"{prefix}{prop.Name}"))
                .ToDictionary(e => e.Key, e => e.Value);
        }
        return dict;
    }
}