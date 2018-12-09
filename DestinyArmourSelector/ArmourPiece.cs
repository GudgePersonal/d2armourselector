using System.Text;

namespace DestinyArmourSelector
{
    public class ArmourPiece
    {
        public ArmourPiece(ArmourType armourType)
        {
            ArmourType = armourType;
        }

        public int RowNumber { get; set; }

        public CharacterClass Class { get; set; }

        public ArmourType ArmourType { get; protected set; }

        public string Name { get; set; }

        public int PowerLevel { get; set; }

        public int MasterWorkLevel { get; set; } // 0 indicates no masterwork (e.g. exotic or other special armour piece)

        public Element Element { get; set; }

        public PerkGroup BasePerks { get; set; }

        public PerkGroup PrimaryPerks { get; set; }

        public PerkGroup SecondaryPerks { get; set; }

        public string Synergy { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Row: {RowNumber}");
            sb.Append(", ");
            sb.Append(CharacterClassHelpers.ToString(Class));
            sb.Append(", ");
            sb.Append(ArmourTypeHelpers.ToString(ArmourType));
            sb.Append(", ");
            sb.Append(Name);
            sb.Append(", ");
            sb.Append(PowerLevel);
            sb.Append(", ");
            sb.Append(Synergy);

            return sb.ToString();
        }
    }
}
