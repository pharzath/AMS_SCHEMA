using AMS.Model.Models;
using AMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Olive;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using ServiceStack;

namespace AMS_SCHEMA.Application.ExtensionMethods
{
    public static class AmsNeo4JNodeLabelExtensions
    {
        public static IEnumerable<AmsNeo4JNodeLabelPropery> PropertiesFull(this AmsNeo4JNodeLabel label,
            JObject? jObject = null)
        {
            var lst = new List<AmsNeo4JNodeLabelPropery>();
            var lbl = label;
            while (lbl is { })
            {
                if(jObject != null)
                    lst.Add(new PropertyFolder(lbl));

                AddProps(lst, lbl.Properties, jObject);
                lbl = lbl.ParentLabel;
            }
            if (jObject != null)
                lst.Add(new PropertyFolder(new AmsNeo4JNodeLabel()
                {
                    Name = "Entity Base",
                    DisplayName = "کلاس مادر",
                }));

            AddProps(lst, EntityBaseClassDef.GetProperties(label), jObject);
            return lst;
        }

        static void AddProps(ICollection<AmsNeo4JNodeLabelPropery> lst, IEnumerable<AmsNeo4JNodeLabelPropery>? props, JObject? jObject)
        {
            if (props == null) return;

            foreach (var prop in props)
            {
                if (jObject != null && prop.Neo4jName != null)
                {
                    var jToken = jObject[prop.Neo4jName];
                    switch (prop.DataType)
                    {
                        case "DateTime":
                            {
                                var convertTo1 = prop.ConvertTo<MyProp<DateTime?>>();
                                convertTo1.Value = jToken?.Value<DateTime?>();
                                lst.Add(convertTo1);
                                break;
                            }
                        case "bool":
                            {
                                var convertTo1 = prop.ConvertTo<MyProp<bool?>>();
                                convertTo1.Value = jToken?.Value<bool?>();
                                lst.Add(convertTo1);
                                break;
                            }
                        case "string":
                            {
                                var convertTo1 = prop.ConvertTo<MyProp<string?>>();
                                convertTo1.Value = jToken?.Value<string?>();
                                lst.Add(convertTo1);
                                break;
                            }
                        case "Guid":
                            {
                                var convertTo1 = prop.ConvertTo<MyProp<Guid?>>();
                                if (Guid.TryParse(jToken?.Value<string>(), out var guid))
                                    convertTo1.Value = guid;
                                lst.Add(convertTo1);
                                break;
                            }
                        case "int":
                            {
                                var convertTo1 = prop.ConvertTo<MyProp<int?>>();
                                convertTo1.Value = jToken?.Value<int?>();
                                lst.Add(convertTo1);
                                break;
                            }

                    }
                }
                else
                {
                    lst.Add(prop);
                }

            }
        }
    }

    public class PropertyFolder : AmsNeo4JNodeLabelPropery
    {
        public AmsNeo4JNodeLabel Label { get; set; }

        public PropertyFolder(AmsNeo4JNodeLabel label)
        {
            Label = label;
        }
    }

    public class MyProp<T> : AmsNeo4JNodeLabelPropery
    {
        [NotMapped]
        public T Value { get; set; }

    }
}
