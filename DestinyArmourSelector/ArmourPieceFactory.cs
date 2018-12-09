// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System.Collections.Generic;
    using System.Linq;

    public class ArmourPieceFactory
    {
        private readonly ArmourType _armourType = ArmourType.Unknown;

        public static ArmourPieceFactory Create(ArmourType armourType)
        {
            return new ArmourPieceFactory(armourType);
        }

        protected ArmourPieceFactory(ArmourType armourType)
        {
            _armourType = armourType;
        }

        public ArmourPiece CreateArmourPiece(int rowNumber, IEnumerable<string> tokens)
        {
            IList<string> tokenList = tokens.ToList();

            CharacterClass characterClass = CharacterClassHelpers.FromString(tokenList[0]);

            string name = tokenList[1];
            int powerLevel = int.Parse(tokenList[2]);
            int masterWork = 0;

            int.TryParse(tokenList[3], out masterWork);

            Element element = ElementHelpers.FromString(tokenList[4]);

            PerkGroup basePerks = new PerkGroup();
            PerkGroup primaryPerks = new PerkGroup();
            PerkGroup secondaryPerks = new PerkGroup();

            string synergy = string.Empty;

            if (_armourType == ArmourType.ClassItem)
            {
                // Class,Name,PL,MW,Element,1,2,3,4,5

                primaryPerks.Perk1 = tokenList[5];
                primaryPerks.Perk2 = tokenList[6];
                primaryPerks.Perk3 = tokenList[7];

                secondaryPerks.Perk1 = tokenList[8];
                secondaryPerks.Perk2 = tokenList[9];
            }
            else
            {
                // Class,Name,PL,MW,Element,1,2,3,4,5,6,7,Synergy

                basePerks.Perk1 = tokenList[5];
                basePerks.Perk2 = tokenList[6];

                primaryPerks.Perk1 = tokenList[7];
                primaryPerks.Perk2 = tokenList[8];
                primaryPerks.Perk3 = tokenList[9];

                secondaryPerks.Perk1 = tokenList[10];
                secondaryPerks.Perk2 = tokenList[11];

                synergy = tokenList[12];
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
