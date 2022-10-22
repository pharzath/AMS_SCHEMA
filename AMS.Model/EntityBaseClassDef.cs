using AMS.Model.Models;
using AMS_SCHEMA.Class;

namespace AMS_SCHEMA.Pages.Schema.EntityBase
{
    public class EntityBaseClassDef
    {
        public static Dictionary<string, PropDef> PropertiesDict => new()
        {
            ["Id"] = new() { Type = "Guid", Description = "شناسه موجودیت" },
            ["Name"] = new() { Type = "string", Description = "نام سیستمی" },
            ["Title"] = new() { Type = "string", Description = "عنوان" },
            ["DisplayName"] = new() { Type = "string", Description = "توضیحات" },
            ["ClientDomainName"] = new() { Type = "string", Description = "دامنه استفاده کاربر" },
            ["CreatedAt"] = new() { Type = "DateTime", Description = "ایجاد شده در تاریخ" },
            ["CreatedBy"] = new() { Type = "Guid", Description = "ایجاد شده توسط" },
            ["ModifiedAt"] = new() { Type = "DateTime", Description = "ویرایش شده در تاریخ" },
            ["ModifiedBy"] = new() { Type = "Guid", Description = "ویرایش شده توسط" },
            ["ModifyAction"] = new() { Type = "string", Description = "علت ایجاد تغییر" },
            ["TransactionLog"] = new() { Type = "string", Description = "لاگ تراکنش" },
            ["IsSoftDeleted"] = new() { Type = "bool", Description = "حذف شده؟" },
            ["IsArchived"] = new() { Type = "bool", Description = "آرشیو شده؟" },
            ["RowAccessPermission"] = new() { Type = "string", Description = "قوانین دسترسی به این رکورد" },
        };

        public static IEnumerable<AmsNeo4JNodeLabelPropery> Properties => GetEntityBaseProperties();

        public static IEnumerable<AmsNeo4JNodeLabelPropery> GetProperties(AmsNeo4JNodeLabel label)
        {
            return GetEntityBaseProperties(label);
        }

        public static IEnumerable<AmsNeo4JNodeLabelPropery> GetEntityBaseProperties(AmsNeo4JNodeLabel? label = null)
        {

            var props = PropertiesDict
                .Select(prop => new AmsNeo4JNodeLabelPropery
                {
                    Label = label,
                    Name = prop.Key,
                    Neo4jName = prop.Key.ToCamelCase(),
                    DataType = prop.Value.Type,
                    DisplayName = prop.Value.Description
                });
            return props;

        }
    }

    public class PropDef
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
