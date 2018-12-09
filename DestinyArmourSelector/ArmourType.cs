// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    public enum ArmourType
    {
        Unknown,
        Helmet,
        Gloves,
        Chest,
        Legs,
        ClassItem
    }

    public static class ArmourTypeHelpers
    {
        public static ArmourType FromString(string s)
        {
            switch (s)
            {
            case "Helmet":
                return ArmourType.Helmet;
            case "Gloves":
                return ArmourType.Gloves;
            case "Chest":
                return ArmourType.Chest;
            case "Legs":
                return ArmourType.Legs;
            case "ClassItem":
                return ArmourType.ClassItem;
            default:
                return ArmourType.Unknown;
            }
        }

        public static string ToString(ArmourType c)
        {
            switch (c)
            {
            case ArmourType.Helmet:
                return "Helmet";
            case ArmourType.Gloves:
                return "Gloves";
            case ArmourType.Chest:
                return "Chest";
            case ArmourType.Legs:
                return "Legs";
            case ArmourType.ClassItem:
                return "ClassItem";
            default:
                return "Unknown";
            }
        }
    }
}
