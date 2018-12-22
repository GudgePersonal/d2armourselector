// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using Interfaces;
    using System.Collections.Generic;

    public class DIMArmourPieceFactory : IArmourPieceFactory
    {
        // 0                1             2                    3         4          5       6           7      8                        9                10          11      12        13    14      15     16          17            18        19        20          21     22 ...
        // Name,            Hash,         Id,                  Tag,      Tier,      Type,   Equippable, Power, Masterwork Type,         Masterwork Tier, Owner,      Locked, Equipped, Year, Season, Event, DTR Rating, # of Reviews, Mobility, Recovery, Resilience, Notes, Perks

        private readonly int _nameIndex = 0;
        private readonly int _typeIndex = 5;
        private readonly int _classIndex = 6;
        private readonly int _powerIndex = 7;
        private readonly int _elementIndex = 8;
        private readonly int _masterworkIndex = 9;
        private readonly int _perksIndex = 22;

        private readonly ArmourType _armourType = ArmourType.Unknown;

        // HashSets used for finding various types of perks
        private static HashSet<string> _basePerkStrings = new HashSet<string>
        {
            "Plasteel Reinforcement Mod", "Restorative Mod", "Mobility Enhancement Mod"
        };

        private static readonly HashSet<string> _primaryPerkStrings = new HashSet<string>
        {
            // Helmet
            "Targeting",
            "Ashes to Assets",
            "Hands-On",
            "Heavy Lifting",
            "Light Reactor",
            "Pump Action",
            "Remote Connection",
            // Gloves
            "Loader",
            "Fastball",
            "Momentum Transfer",
            "Impact Induction",
            // Chest    
            "Unflinching",
            // Legs
            "Dexterity",
            "Traction",
            "Perpetuation",
            "Dynamo",
            "Bomber",
            "Outreach",
            "Distribution",
            // Class Item
            "Insulation",
            "Innervation",
            "Perpetuation",
            "Invigoration",
            "Better Already",
            "Recuperation",
            "Absolution"
        };

        private static readonly HashSet<string> _secondaryPerkStrings = new HashSet<string>
        {
            "Finder",
            "Reserves",
            "Scavenger"
        };

        // HashSets from here down are currently unused. Kept around partly for documentation purposes and partly 
        // because I think I should be able to generate the HashSets above from these somehow...
        private static readonly HashSet<string> _kineticWeapons = new HashSet<string>
        {
            "Auto Rifle",
            "Scout Rifle",
            "Pulse Rifle",
            "Hand Cannon",
            "Submachine Gun",
            "Sidearm",
            "Bow",
            "Shotgun",
            "Grenade Launcher",
            "Sniper Rifle"
        };

        private static readonly HashSet<string> _energyWeapons = new HashSet<string>
        {
            "Auto Rifle",
            "Scout Rifle",
            "Pulse Rifle",
            "Hand Cannon",
            "Submachine Gun",
            "Sidearm",
            "Bow",
            "Shotgun",
            "Grenade Launcher",
            "Fusion Rifle",
            "Sniper Rifle",
            "Trace Rifle"
        };

        private static readonly HashSet<string> _powerWeapons = new HashSet<string>
        {
            "Sword",
            "Grenade Launcher",
            "Rocket Launcher",
            "Linear Fusion",
            "Machine Gun",
            "Sniper Rifle",
            "Shotgun",
        };

        private static readonly HashSet<string> _primaryWeapons = new HashSet<string>
        {
            "Auto Rifle",
            "Scout Rifle",
            "Pulse Rifle",
            "Hand Cannon",
            "Submachine Gun",
            "Sidearm",
            "Bow"
        };

        private static readonly HashSet<string> _specialWeapons = new HashSet<string>
        {
            "Shotgun",
            "Grenade Launcher",
            "Fusion Rifle",
            "Sniper Rifle",
            "Trace Rifle"
        };

        private static readonly HashSet<string> _heavyWeapons = new HashSet<string>
        {
            "Sword",
            "Grenade Launcher",
            "Rocket Launcher",
            "Linear Fusion",
            "Machine Gun",
            "Sniper Rifle",
            "Shotgun",
        };

        private static readonly HashSet<string> _oversizeWeapons = new HashSet<string>
        {
            "Rocket Launcher",
            "Grenade Launcher",
            "Shotgun",
            "Bow"
        };

        private static readonly HashSet<string> _lightArmsWeapons = new HashSet<string>
        {
            "Hand Cannon",
            "Sidearm",
            "Submachine Gun"
        };

        private static readonly HashSet<string> _scatterProjectileWeapons = new HashSet<string>
        {
            "Auto Rifle",
            "Pulse Rifle",
            "Submachine Gun",
            "Sidearm",
            "Fusion Rifle"
        };

        private static readonly HashSet<string> _precisionWeapons = new HashSet<string>
        {
            "Hand Cannon",
            "Scout Rifle",
            "Trace Rifle",
            "Bow",
            "Linear Fusion",
            "Sniper Rifle",
            "Slug Shotgun"
        };

        private static readonly HashSet<string> _rifleWeapons = new HashSet<string>
        {
            "Auto Rifle",
            "Pulse Rifle",
            "Scout Rifle",
            "Trace Rifle",
            "Fusion Rifle",
            "Linear Fusion",
            "Sniper Rifle"
        };

        public static IArmourPieceFactory Create(ArmourType armourType)
        {
            return new DIMArmourPieceFactory(armourType);
        }

        protected DIMArmourPieceFactory(ArmourType armourType)
        {
            _armourType = armourType;
        }

        // 0                1             2                    3         4          5       6           7      8                        9                10          11      12        13    14      15     16          17            18        19        20          21     22 ...
        // Name,            Hash,         Id,                  Tag,      Tier,      Type,   Equippable, Power, Masterwork Type,         Masterwork Tier, Owner,      Locked, Equipped, Year, Season, Event, DTR Rating, # of Reviews, Mobility, Recovery, Resilience, Notes, Perks
        // "Prodigal Helm", "2753581141", 6917529083638648522, favorite, Legendary, Helmet, titan,      619,   Solar Damage Resistance, 4,               Titan(623), true,   false,    2,    4,      ,      N/A,        N/A,          1,        1,        1,          ,      Restorative Titan Armor*,Mobility Enhancement Mod*,Restorative Mod, Tier 4 Armor*,Scatter Projectile Targeting*,Machine Gun Targeting,Shotgun Reserves*, Machine Gun Reserves
        public ArmourPiece CreateArmourPiece(int rowNumber, string[] tokens)
        {
            ArmourType armourType = GetArmourType(tokens);

            if (armourType != _armourType)
            {
                return null;
            }

            CharacterClass characterClass = GetCharacterClass(tokens);
            string name = GetName(tokens);
            int powerLevel = GetPowerLevel(tokens);

            int masterWork = GetMasterWorkLevel(tokens);
            Element element = GetMasterWorkElement(tokens);

            var temp = new List<string>();

            for (int i = _perksIndex; i < tokens.Length; ++i)
            {
                temp.Add(tokens[i]);
            }

            string[] perkTokens = temp.ToArray();

            TrimPerkTokens(perkTokens);

            int indexOfFirstBasePerk = FindIndexOfFirstBasePerk(perkTokens);
            int indexOfLastBasePerk = FindIndexOfLastBasePerk(indexOfFirstBasePerk, perkTokens);

            var basePerks = new PerkGroup();

            for (int i = indexOfFirstBasePerk; i <= indexOfLastBasePerk; ++i)
            {
                switch (i - indexOfFirstBasePerk)
                {
                case 0:
                    basePerks.Perk1 = perkTokens[i];
                    break;
                case 1:
                    basePerks.Perk2 = perkTokens[i];
                    break;
                case 2:
                    basePerks.Perk3 = perkTokens[i];
                    break;
                default:
                    break;
                }
            }

            var primaryPerks = new PerkGroup();

            int indexOfFirstPrimaryPerk = FindIndexOfFirstPrimaryPerk(indexOfLastBasePerk, perkTokens);
            int indexOfFirstSecondaryPerk = FindIndexOfFirstSecondaryPerk(indexOfFirstPrimaryPerk, perkTokens);
            int indexOfLastPrimaryPerk = indexOfFirstSecondaryPerk - 1;
            int indexOfLastSecondaryPerk = FindIndexOfLastSecondaryPerk(indexOfFirstSecondaryPerk, perkTokens);

            for (int i = indexOfFirstPrimaryPerk; i <= indexOfLastPrimaryPerk; ++i)
            {
                switch (i - indexOfFirstPrimaryPerk)
                {
                case 0:
                    primaryPerks.Perk1 = perkTokens[i];
                    break;
                case 1:
                    primaryPerks.Perk2 = perkTokens[i];
                    break;
                case 2:
                    primaryPerks.Perk3 = perkTokens[i];
                    break;
                default:
                    break;
                }
            }

            var secondaryPerks = new PerkGroup();

            for (int i = indexOfFirstSecondaryPerk; i <= indexOfLastSecondaryPerk; ++i)
            {
                switch (i - indexOfFirstSecondaryPerk)
                {
                case 0:
                    secondaryPerks.Perk1 = perkTokens[i];
                    break;
                case 1:
                    secondaryPerks.Perk2 = perkTokens[i];
                    break;
                default:
                    break;
                }
            }

            return new ArmourPiece(armourType, new SimpleSynergyCalculator())
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
            };
        }

        private int FindIndexOfFirstBasePerk(string[] perkTokens)
        {
            for (int i = 0; i < perkTokens.Length; ++i)
            {
                if (_basePerkStrings.Contains(perkTokens[i]))
                {
                    return i;
                }
            }

            return 0;
        }

        private int FindIndexOfFirstPrimaryPerk(int indexOfLastBasePerk, string[] perkTokens)
        {
            for (int i = indexOfLastBasePerk + 1; i < perkTokens.Length; ++i)
            {
                foreach (string perk in _primaryPerkStrings)
                {
                    if (perkTokens[i].Contains(perk))
                    {
                        return i;
                    }
                }
            }

            return 0;
        }

        private int FindIndexOfFirstSecondaryPerk(int startIndex, string[] perkTokens)
        {
            for (int i = startIndex + 1; i < perkTokens.Length; ++i)
            {
                foreach (string perk in _secondaryPerkStrings)
                {
                    if (perkTokens[i].Contains(perk))
                    {
                        return i;
                    }
                }
            }

            return 0;
        }

        private int FindIndexOfLastBasePerk(int indexOfFirstBasePerk, string[] perkTokens)
        {
            for (int i = indexOfFirstBasePerk + 1; i < perkTokens.Length; ++i)
            {
                if (!_basePerkStrings.Contains(perkTokens[i]))
                {
                    return i - 1;
                }
            }

            return 0;
        }

        private int FindIndexOfLastPrimaryPerk(int indexOfFirstPrimaryPerk, string[] perkTokens)
        {
            int i;

            for (i = indexOfFirstPrimaryPerk + 1; i < perkTokens.Length; ++i)
            {
                foreach (string perk in _primaryPerkStrings)
                {
                    if (perkTokens[i].Contains(perk))
                    {
                        break;
                    }
                }
            }

            return i;
        }

        private int FindIndexOfLastSecondaryPerk(int startIndex, string[] perkTokens)
        {
            int result = startIndex;

            for (int i = startIndex + 1; i < perkTokens.Length; ++i)
            {
                foreach (string perk in _secondaryPerkStrings)
                {
                    if (perkTokens[i].Contains(perk))
                    {
                        result = i;
                        break;
                    }
                }
            }

            return result;
        }

        private ArmourType GetArmourType(string[] tokens)
        {
            return ArmourTypeHelpers.FromDIMString(tokens[_typeIndex]);
        }

        private CharacterClass GetCharacterClass(string[] tokens)
        {
            return CharacterClassHelpers.FromString(tokens[_classIndex]);
        }

        private Element GetMasterWorkElement(string[] tokens)
        {
            string element = tokens[_elementIndex];
            if (string.IsNullOrWhiteSpace(element))
            {
                return Element.None;
            }

            element = element.Substring(0, element.IndexOf(' '));
            return ElementHelpers.FromString(element);
        }

        private int GetMasterWorkLevel(string[] tokens)
        {
            string masterWorkLevel = tokens[_masterworkIndex];
            if (string.IsNullOrWhiteSpace(masterWorkLevel))
            {
                return 0;
            }

            return int.Parse(masterWorkLevel);
        }

        private string GetName(string[] tokens)
        {
            return tokens[_nameIndex].Trim('"');
        }

        private int GetPowerLevel(string[] tokens)
        {
            return int.Parse(tokens[_powerIndex]);
        }

        private void TrimPerkTokens(string[] perkTokens)
        {
            for (int i = 0; i < perkTokens.Length; ++i)
            {
                perkTokens[i] = perkTokens[i].Trim();
                perkTokens[i] = perkTokens[i].Trim('*');
            }
        }
    }
}
