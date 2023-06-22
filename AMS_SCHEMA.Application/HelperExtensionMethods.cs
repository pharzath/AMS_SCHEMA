using AMS.Model.Models;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using Olive;


namespace AMS_SCHEMA.Class
{
    public static class HelperExtensionMethods
    {
        public static string ToPascalCase(this string s)
        {
            if (s.IsEmpty()) return s;

            var words = s.Split('_', ' ');
            var w = words.Select(x => x[0].ToUpper() + xxx(x[1..]));
            return string.Join(null, w);
        }

        static string xxx(string s)
        {
            var camelCaseId = s.ToCamelCaseId();
            if (s.All(x => x.IsUpper()))
                return s.ToLower();
            return camelCaseId;
        }

        public static string ToCamelCase(this string s)
        {
            var words = s.Split('_', ' ');
            var w = words.Select(x => x.ToPascalCase()).ToArray();

            var camelCase = string.Join(null, w);
            if (!camelCase.IsEmpty())
                camelCase = camelCase[0].ToLower() + camelCase[1..];
            return camelCase;
        }

        public static string ToShortVariableName(this string? s)
        {
            if (s.Contains("_"))
            {
                return string.Concat(s.Split('_').Select(x => x[0])).ToLower();
            }
            var v = "";
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i].IsUpper())
                    v += s[i].ToLower();
            }
            return OliveExtensions.IsEmpty(v) ? s[0].ToString() : v;
        }

        public static string GetColumnDataType(this string tpy)
        {
            switch (tpy)
            {
                case "date":
                case "datetime":
                    return "DateTime";
                case "guid":
                    return "Guid";
                case "boolean":
                    return "bool";
                case "integer":
                    return "int";
                case "text":
                case "longtext":
                    return "string";
            }
            return tpy;
        }
        public static T ConvertTo<T>(this object from)
        {
            // Check if from is null.
            if (from == null)
            {
                return default(T);
            }

            // Create an AutoMapper configuration.
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<object, T>();
            });

            // Create an AutoMapper mapper.
            var mapper = config.CreateMapper();

            // Convert the object to the target type.
            return mapper.Map<object, T>(from);
        }
    }
}
