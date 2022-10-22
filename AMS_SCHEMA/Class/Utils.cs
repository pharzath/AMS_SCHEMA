using AMS.Model.Models;
using AMS.Model.Services;
using MudBlazor;
using Olive;

namespace AMS_SCHEMA.Class
{
    public class Utils
    {
        public static string GetRelationTypePropertyIcon(AmsNeo4JNodeRelationType relType, AmsNeo4JNodeRelationPropery prop)
        {
            string icon = "";
            var constraint = Enumerable.FirstOrDefault(relType.Constraints, x => x.Over == prop.Name);
            if (constraint is { })
            {

                switch (constraint.Type)
                {
                    case "IS NOT NULL":
                        icon = Icons.Filled.BatteryChargingFull;
                        break;
                    case "IS UNIQUE":
                        icon = Icons.Filled.LooksOne;
                        break;
                    case "IS NODE KEY":
                        icon = Icons.Filled.Key;
                        break;

                }
            }
            var index = Enumerable.FirstOrDefault(relType.Indices, x => x.Over == prop.Name);
            if (index is { })
            {
                if (!string.IsNullOrEmpty(icon))
                    return Icons.Filled.LooksTwo;

                icon = Icons.Filled.BookmarkAdded;
            }

            return icon;
        }
        public static string GetRelationTypePropertyBgColor(AmsNeo4JNodeRelationType relType, AmsNeo4JNodeRelationPropery prop)
        {
            var style = "";
            /*
            switch (prop.DataType)
            {
                case "string":
                    style = "background-color: darkgreen;";
                    break;
                case "int":
                    style = "background-color: darkblue;";
                    break;
                case "bool":
                    style = "background-color: darkred;";
                    break;
            }
            */
            if (relType.Constraints.Any(x => x.Over == prop.Name))
            {
                style = "background-color: #ff52006b;";
            }
            var index = Enumerable.FirstOrDefault(relType.Indices, x => x.Over == prop.Name);
            if (index is { })
            {
                style = "background-color: #ee98106b;";
            }
            return style;
        }
        public static string GetLabelPropertyIcon(AmsNeo4JNodeLabel? label, AmsNeo4JNodeLabelPropery prop)
        {
            var icon = "";

            if (label == null)
                return icon;

            var constraint = Enumerable.FirstOrDefault(label.Constraints, x => x.Over == prop.Name);
            if (constraint is { })
            {

                switch (constraint.Type)
                {
                    case "IS NOT NULL":
                        icon = Icons.Filled.BatteryChargingFull;
                        break;
                    case "IS UNIQUE":
                        icon = Icons.Filled.LooksOne;
                        break;
                    case "IS NODE KEY":
                        icon = Icons.Filled.Key;
                        break;

                }
            }
            var index = Enumerable.FirstOrDefault(label.Indices, x => x.Over == prop.Name);
            if (index is { })
            {
                if (!string.IsNullOrEmpty(icon))
                    return Icons.Filled.LooksTwo;

                icon = Icons.Filled.BookmarkAdded;
            }

            return icon;
        }


        public static string GetLabelPropertyBgColor(AmsNeo4JNodeLabel? label, AmsNeo4JNodeLabelPropery prop )
        {
            var style = "";
            /*
            switch (prop.DataType)
            {
                case "string":
                    style = "background-color: darkgreen;";
                    break;
                case "int":
                    style = "background-color: darkblue;";
                    break;
                case "bool":
                    style = "background-color: darkred;";
                    break;
            }
            */

            if (label == null)
                return style;

            if (label.Constraints.Any(x => x.Over == prop.Name))
            {
                style = "background-color: #ff52006b;";
            }
            var index = Enumerable.FirstOrDefault(label.Indices, x => x.Over == prop.Name);
            if (index is { })
            {
                style = style.IsEmpty() ?
                    "background-color: #ee98106b;" :
                    "background-color: darkblue;";
            }
            return style;
        }

        public static HashSet<AmsNeo4JNodeLabelTreenNode> GetNodes(DataService ds, AmsNeo4JNode amsNeo4JNode)
        {
            var root = ds.GetRootNodeLabels(amsNeo4JNode);
            foreach (var lbl in root)
            {
                PrepareChildren(ds, lbl);
            }
            return root;

        }

        public static void PrepareChildren(DataService ds, AmsNeo4JNodeLabelTreenNode parent)
        {
            parent.Items = ds.GetChildLabels(parent);
            foreach (var lbl in parent.Items)
            {
                PrepareChildren(ds, lbl);
            }
        }

    }
}
