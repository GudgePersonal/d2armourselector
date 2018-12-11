// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    public class DIMArmourPieceFactory : IArmourPieceFactory
    {
        public static IArmourPieceFactory Create(ArmourType armourType)
        {
            return new DIMArmourPieceFactory();
        }

        protected DIMArmourPieceFactory()
        {
        }

        // 0    1    2  3   4    5    6          7     8     9      10       11   12     13    14         15           16       17       18         19    20    ...
        // Name,Hash,Id,Tag,Tier,Type,Equippable,Power,Owner,Locked,Equipped,Year,Season,Event,DTR Rating,# of Reviews,Mobility,Recovery,Resilience,Notes,Perks ...
        // "Reverie Dawn Helm","4097166900",6917529083913248847,,Legendary,Helmet,titan,603,Titan(609),true,false,2,4,,N/A,N/A,2,0,1,,Mobile Titan Armor*,Mobility Enhancement Mod*,Restorative Mod, Riven's Curse*,Tier 2 Armor*,Burnished Dreams*,Pulse Rifle Targeting*,Light Reactor,Ashes to Assets,Fusion Rifle Reserves,Shotgun Reserves*
        //
        public ArmourPiece CreateArmourPiece(int rowNumber, string[] tokens)
        {
            CharacterClass characterClass = GetCharacterClass(tokens);
            string name = GetName(tokens);
            int powerLevel = GetPowerLevel(tokens);
            ArmourType armourType = GetArmourType(tokens);

            // Can't get Masterwork level or Element from DIM CSV
            int masterWork = 0;
            Element element = Element.None;

            var basePerks = new PerkGroup();
            var primaryPerks = new PerkGroup();
            var secondaryPerks = new PerkGroup();

            // TODO: Populate PerkGroups

            string synergy = string.Empty;

            // TODO: Calculate synergy

            return new ArmourPiece(armourType)
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

        private ArmourType GetArmourType(string[] tokens)
        {
            return ArmourTypeHelpers.FromDIMString(tokens[5]);
        }

        private CharacterClass GetCharacterClass(string[] tokens)
        {
            return CharacterClassHelpers.FromString(tokens[6]);
        }

        private string GetName(string[] tokens)
        {
            return tokens[0].Trim('"');
        }

        private int GetPowerLevel(string[] tokens)
        {
            return int.Parse(tokens[7]);
        }
    }
}
