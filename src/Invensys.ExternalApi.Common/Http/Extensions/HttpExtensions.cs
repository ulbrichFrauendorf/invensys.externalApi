using System.Reflection;
using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.Common.Http.Extensions;

public static class FormDataExtensions
{
    public static FormUrlEncodedContent ToFormData(this object obj)
    {
        var formData = new Dictionary<string, string>();

        foreach (var property in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var jsonPropertyName = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? property.Name;
            var value = property.GetValue(obj)?.ToString();
            if (value != null)
            {
                formData.Add(jsonPropertyName, value);
            }
        }

        return new FormUrlEncodedContent(formData);
    }
}