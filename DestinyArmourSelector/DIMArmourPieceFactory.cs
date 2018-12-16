// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class DIMArmourPieceFactory : IArmourPieceFactory
    {
        private readonly ArmourType _armourType = ArmourType.Unknown;
        private static string _unflinchingAimPrefix = "Unflinching ";

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
            // Glove
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

        static readonly HashSet<string> _secondaryPerkStrings = new HashSet<string>
        {
            "Finder",
            "Reserves",
            "Scavenger"
        };


        // HashSets used for tracking perk synergy for each weapon type
        private static readonly HashSet<string> _autoRifleSynergy = new HashSet<string>
        {
            "Auto Rifle",
            "Kinetic",
            "Energy",
            "Scatter",
            "Rifle"
        };

        private static readonly HashSet<string> _scoutRifleSynergy = new HashSet<string>
        {
            "Scout Rifle",
            "Kinetic",
            "Energy",
            "Precision",
            "Rifle"
        };

        private static readonly HashSet<string> _pulseRifleSynergy = new HashSet<string>
        {
            "Scout Rifle",
            "Kinetic",
            "Energy",
            "Scatter",
            "Rifle"
        };

        private static readonly HashSet<string> _handCannonSynergy = new HashSet<string>
        {
            "Hand Cannon",
            "Kinetic",
            "Energy",
            "Precision",
            "Light"
        };

        private static readonly HashSet<string> _subMachineGunSynergy = new HashSet<string>
        {
            "Submachine Gun",
            "Kinetic",
            "Energy",
            "Scatter",
            "Light"
        };

        private static readonly HashSet<string> _sidearmSynergy = new HashSet<string>
        {
            "Sidearm",
            "Kinetic",
            "Energy",
            "Scatter",
            "Light"
        };

        private static readonly HashSet<string> _bowSynergy = new HashSet<string>
        {
            "Bow",
            "Kinetic",
            "Energy",
            "Precision",
            "Oversize"
        };

        private static readonly HashSet<string> _shotgunSynergy = new HashSet<string>
        {
            "Shotgun",
            "Kinetic",
            "Energy",
            "Power",
            "Pump Action",
            "Oversize"
        };

        private static readonly HashSet<string> _grenadeLauncherSynergy = new HashSet<string>
        {
            "Grenade Launcher",
            "Kinetic",
            "Energy",
            "Power",
            "Oversize"
        };

        private static readonly HashSet<string> _fusionRifleSynergy = new HashSet<string>
        {
            "Fusion Rifle",
            "Energy",
            "Scatter",
            "Rifle"
        };


        private static readonly HashSet<string> _sniperRifleSynergy = new HashSet<string>
        {
            "Sniper Rifle",
            "Kinetic",
            "Energy",
            "Power",
            "Precision",
            "Rifle"
        };

        private static readonly HashSet<string> _traceRifleSynergy = new HashSet<string>
        {
            "Trace Rifle",
            "Energy",
            "Precision",
            "Rifle"
        };

        private static readonly HashSet<string> _swordSynergy = new HashSet<string>
        {
            "Sword",
            "Power"
        };

        private static readonly HashSet<string> _rocketLauncherSynergy = new HashSet<string>
        {
            "Rocket Launcher",
            "Power",
            "Oversize"
        };

        private static readonly HashSet<string> _linearFusionSynergy = new HashSet<string>
        {
            "Linear Fusion",
            "Power",
            "Precision",
            "Rifle"
        };

        private static readonly HashSet<string> _machineGunSynergy = new HashSet<string>
        {
            "Machine Gun",
            "Power",
        };

        private static readonly HashSet<string> _helmetPrimaryPerkStrings = new HashSet<string>
        {
            "Targeting",
            "Ashes to Assets",
            "Hands-On",
            "Heavy Lifting",
            "Light Reactor",
            "Pump Action",
            "Remote Connection",
        };

        private static readonly HashSet<string> _glovePrimaryPerkStrings = new HashSet<string>
        {
            "Loader",
            "Fastball",
            "Momentum Transfer",
            "Impact Induction",
        };

        private static readonly HashSet<string> _chestPrimaryPerkStrings = new HashSet<string>
        {
            "Unflinching",
        };

        private static readonly HashSet<string> _legPrimaryPerkStrings = new HashSet<string>
        {
            "Dexterity",
            "Traction",
            "Perpetuation",
            "Dynamo",
            "Bomber",
            "Outreach",
            "Distribution",
        };

        private static readonly HashSet<string> _classItemPrimaryPerkStrings = new HashSet<string>
        {
            "Insulation",
            "Innervation",
            "Perpetuation",
            "Invigoration",
            "Better Already",
            "Recuperation",
            "Absolution"
        };


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

        // 0    1    2  3   4    5    6          7     8     9      10       11   12     13    14         15           16       17       18         19    20    ...
        // Name,Hash,Id,Tag,Tier,Type,Equippable,Power,Owner,Locked,Equipped,Year,Season,Event,DTR Rating,# of Reviews,Mobility,Recovery,Resilience,Notes,Perks ...
        // "Reverie Dawn Helm","4097166900",6917529083913248847,,Legendary,Helmet,titan,603,Titan(609),true,false,2,4,,N/A,N/A,2,0,1,,Mobile Titan Armor*,Mobility Enhancement Mod*,Restorative Mod, Riven's Curse*,Tier 2 Armor*,Burnished Dreams*,Pulse Rifle Targeting*,Light Reactor,Ashes to Assets,Fusion Rifle Reserves,Shotgun Reserves*
        //
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

            // Can't get Masterwork level or Element from DIM CSV
            int masterWork = tokens[4].Equals("Exotic") ? 0 : 1;
            Element element = Element.None;

            var temp = new List<string>();

            for (int i = 20; i < tokens.Length; ++i)
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

            string synergy = CalculateSynergy(primaryPerks, secondaryPerks);

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


        private string CalculateSynergy(PerkGroup primaryPerks, PerkGroup secondaryPerks)
        {
            var sb = new StringBuilder();

            if(!string.IsNullOrWhiteSpace(primaryPerks.Perk1))
            {
                sb.Append(CalculateSynergy(1, primaryPerks.Perk1, secondaryPerks));
            }

            if (!string.IsNullOrWhiteSpace(primaryPerks.Perk2))
            {
                sb.Append(CalculateSynergy(2, primaryPerks.Perk2, secondaryPerks));
            }

            if (!string.IsNullOrWhiteSpace(primaryPerks.Perk3))
            {
                sb.Append(CalculateSynergy(3, primaryPerks.Perk3, secondaryPerks));
            }

            return sb.ToString();
        }

        private string CalculateSynergy(int index, string primaryPerk, PerkGroup secondaryPerks)
        {
            string result = string.Empty;

            switch (_armourType)
            {
            case ArmourType.Helmet:
                result = CalculateHelmetSynergy(index, primaryPerk, secondaryPerks);
                break;
            case ArmourType.Gloves:
                result = CalculateGloveSynergy(index, primaryPerk, secondaryPerks);
                break;
            case ArmourType.Chest:
                result = CalculateChestSynergy(index, primaryPerk, secondaryPerks);
                break;
            case ArmourType.Legs:
                result = CalculateLegSynergy(index, primaryPerk, secondaryPerks);
                break;
            case ArmourType.ClassItem:
                break;
            default:
                break;
            }

            return result;
        }

        private string CalculateLegSynergy(int index, string primaryPerk, PerkGroup secondaryPerks)
        {
            var sb = new StringBuilder();

            
            return sb.ToString();
        }

        private string CalculateChestSynergy(int index, string primaryPerk, PerkGroup secondaryPerks)
        {
            var sb = new StringBuilder();

            
            return sb.ToString();
        }

        private string CalculateGloveSynergy(int index, string primaryPerk, PerkGroup secondaryPerks)
        {
            var sb = new StringBuilder();

            

            return sb.ToString();
        }

        private string CalculateHelmetSynergy(int index, string primaryPerk, PerkGroup secondaryPerks)
        {
            var sb = new StringBuilder();

            sb.Append(CalculatePrimarySecondaryPerkSynergy(index, primaryPerk, "Pump Action", secondaryPerks, "Shotgun"));
            sb.Append(CalculatePrimarySecondaryPerkSynergy(index, primaryPerk, "Light Reactor", secondaryPerks, "Fusion Rifle"));
            sb.Append(CalculatePrimarySecondaryPerkSynergy(index, primaryPerk, "Remote Connection", secondaryPerks, "Sniper Rifle"));

            foreach (string weaponText in _heavyWeapons)
            {
                sb.Append(CalculatePrimarySecondaryPerkSynergy(index, primaryPerk, "Heavy Lifting", secondaryPerks, weaponText));
            }

            sb.Append(CalculatePrimarySecondaryPerkSynergy(index, primaryPerk, "Heavy Lifting", secondaryPerks, "Heavy Ammo"));

            if (primaryPerk.Contains("Targeting"))
            {
                string weaponText = primaryPerk.Substring(0, primaryPerk.IndexOf("Targeting"));
                weaponText = weaponText.Trim();

                if(weaponText.Equals(""))

                if(secondaryPerks.Perk1.Contains(weaponText))
                {
                    sb.Append($"{index}_1");
                }

                if (!string.IsNullOrWhiteSpace(secondaryPerks.Perk2) && secondaryPerks.Perk2.Contains(weaponText))
                {
                    sb.Append($"{index}_2");
                }                
            }

            return sb.ToString();
        }

        private string CalculatePrimarySecondaryPerkSynergy(int index, string primaryPerk, string primaryPerkText, PerkGroup secondaryPerks, string weaponText)
        {
            var sb = new StringBuilder();

            if (primaryPerk.Equals(primaryPerkText))
            {
                if (secondaryPerks.Perk1.StartsWith(weaponText))
                {
                    sb.Append($"{index}_1");
                }

                if (!string.IsNullOrWhiteSpace(secondaryPerks.Perk2) && secondaryPerks.Perk2.StartsWith(weaponText))
                {
                    sb.Append($"{index}_2");
                }
            }

            return sb.ToString();
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

        private void TrimPerkTokens(string[] perkTokens)
        {
            for (int i = 0; i < perkTokens.Length; ++i)
            {
                perkTokens[i] = perkTokens[i].Trim();
                perkTokens[i] = perkTokens[i].Trim('*');
            }
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
