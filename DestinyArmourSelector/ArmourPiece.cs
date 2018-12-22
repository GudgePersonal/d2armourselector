// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using Interfaces;
    using System.Text;

    public class ArmourPiece
    {
        ISynergyCalculator _synergyCalculator;
        string _synergy = null;

        public ArmourPiece(ArmourType armourType, ISynergyCalculator synergyCalculator)
        {
            _synergyCalculator = synergyCalculator;
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

        public string Synergy
        {
            get
            {
                if (_synergy == null)
                {
                    _synergy = _synergyCalculator.CalculateSynergy(PrimaryPerks, SecondaryPerks);
                }

                return _synergy;
            }
        }

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
            sb.Append(ElementHelpers.ToString(Element));
            sb.Append(", ");
            sb.Append(MasterWorkLevel);
            sb.Append(", ");
            sb.Append(Synergy);

            return sb.ToString();
        }
    }
}
