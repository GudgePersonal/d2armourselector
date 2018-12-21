// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    public enum Element
    {
        None,
        Arc,
        Solar,
        Void
    }

    public static class ElementHelpers
    {
        public static Element FromString(string s)
        {
            switch (s)
            {
            case "Arc":
                return Element.Arc;
            case "Solar":
                return Element.Solar;
            case "Void":
                return Element.Void;
            default:
                return Element.None;
            }
        }

        public static string ToString(Element element)
        {
            switch (element)
            {
            case Element.Arc:
                return "Arc";
            case Element.Solar:
                return "Solar";
            case Element.Void:
                return "Void";
            default:
                return "None";
            }
        }
    }
}
