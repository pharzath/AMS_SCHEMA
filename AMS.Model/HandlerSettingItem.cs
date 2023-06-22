using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Olive;


public class HandlerSettingItem
{
    public enum HttpMethodEnum { HttpGet, HttpPut, HttpPost, HttpDelete }

    [JsonConverter(typeof(StringEnumConverter))]
    public HttpMethodEnum HttpMethod { get; set; }

    public string Name { get; set; }

    public string HandlerNameFormat { get; set; }

    public List<MethodParameter> Parameters { get; set; }

    public MethodParameter ReturnType { get; set; }

    public override string ToString()
    {
        return Name.ToPascalCaseId();
    }
}

public class MethodParameter
{
    public bool IsCollection { get; set; }
    public bool IsSelfDto { get; set; }
    public string Type { get; set; }

    public string? Name { get; set; }
    public string? Default { get; set; }

    public override string ToString()
    {
        return Type + " " + Name + Default.WithWrappers(" = ", "");
    }
}