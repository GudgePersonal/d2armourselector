// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    public class ArmourPieceFactory : IArmourPieceFactory
    {
        private readonly ArmourType _armourType = ArmourType.Unknown;

        public static IArmourPieceFactory Create(ArmourType armourType)
        {
            return new ArmourPieceFactory(armourType);
        }

        protected ArmourPieceFactory(ArmourType armourType)
        {
            _armourType = armourType;
        }

        public ArmourPiece CreateArmourPiece(int rowNumber, string[] tokens)
        {
            CharacterClass characterClass = CharacterClassHelpers.FromString(tokens[0]);

            string name = tokens[1];
            int powerLevel = int.Parse(tokens[2]);
            int masterWork = 0;

            int.TryParse(tokens[3], out masterWork);

            Element element = ElementHelpers.FromString(tokens[4]);

            PerkGroup basePerks = new PerkGroup();
            PerkGroup primaryPerks = new PerkGroup();
            PerkGroup secondaryPerks = new PerkGroup();

            string synergy = string.Empty;

            if (_armourType == ArmourType.ClassItem)
            {
                // 0     1    2  3  4       5 6 7 8 9
                // Class,Name,PL,MW,Element,1,2,3,4,5

                primaryPerks.Perk1 = tokens[5];
                primaryPerks.Perk2 = tokens[6];
                primaryPerks.Perk3 = tokens[7];

                secondaryPerks.Perk1 = tokens[8];
                secondaryPerks.Perk2 = tokens[9];
            }
            else
            {
                //                                    1 1 1
                // 0     1    2  3  4       5 6 7 8 9 0 1 2
                // Class,Name,PL,MW,Element,1,2,3,4,5,6,7,Synergy

                basePerks.Perk1 = tokens[5];
                basePerks.Perk2 = tokens[6];

                primaryPerks.Perk1 = tokens[7];
                primaryPerks.Perk2 = tokens[8];
                primaryPerks.Perk3 = tokens[9];

                secondaryPerks.Perk1 = tokens[10];
                secondaryPerks.Perk2 = tokens[11];

                synergy = tokens[12];
            }

            return new ArmourPiece(_armourType)
            {
                BasePerks = basePerks,
                Class = characterClass,
                Element = element,
                MasterWorkLevel = masterWork,
                Name = name,
                PowerLevel = powerLevel,
                PrimaryPerks = primaryPerks,
                RowNumber = rowNumber,
                SecondaryPerks = secondaryPerks,
                Synergy = synergy
            };
        }
    }
}
