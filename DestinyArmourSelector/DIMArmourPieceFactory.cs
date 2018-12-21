// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class DIMArmourPieceFactory : IArmourPieceFactory
    {
        // 0                1             2                    3         4          5       6           7      8                        9                10          11      12        13    14      15     16          17            18        19        20          21     22 ...
        // Name,            Hash,         Id,                  Tag,      Tier,      Type,   Equippable, Power, Masterwork Type,         Masterwork Tier, Owner,      Locked, Equipped, Year, Season, Event, DTR Rating, # of Reviews, Mobility, Recovery, Resilience, Notes, Perks

        private int _nameIndex = 0;
        private int _typeIndex = 5;
        private int _classIndex = 6;
        private int _powerIndex = 7;
        private int _elementIndex = 8;
        private int _masterworkIndex = 9;
        private int _perksIndex = 22;

        private readonly ArmourType _armourType = ArmourType.Unknown;
        private static readonly string _enhancedPrefix = "Enhanced ";
        private static readonly string _unflinchingAimPrefix = "Unflinching ";

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
            "Light Arms"
        };

        private static readonly HashSet<string> _subMachineGunSynergy = new HashSet<string>
        {
            "Submachine Gun",
            "Kinetic",
            "Energy",
            "Scatter",
            "Light Arms"
        };

        private static readonly HashSet<string> _sidearmSynergy = new HashSet<string>
        {
            "Sidearm",
            "Kinetic",
            "Energy",
            "Scatter",
            "Light Arms"
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
            "Oversize",
            "Heavy Lifting"
        };

        private static readonly HashSet<string> _fusionRifleSynergy = new HashSet<string>
        {
            "Fusion Rifle",
            "Energy",
            "Scatter",
            "Rifle",
            "Light Reactor",
        };

        private static readonly HashSet<string> _sniperRifleSynergy = new HashSet<string>
        {
            "Sniper Rifle",
            "Kinetic",
            "Energy",
            "Power",
            "Precision",
            "Rifle",
            "Remote Connection",
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
            "Power",
            "Heavy Lifting"
        };

        private static readonly HashSet<string> _rocketLauncherSynergy = new HashSet<string>
        {
            "Rocket Launcher",
            "Power",
            "Oversize",
            "Heavy Lifting"
        };

        private static readonly HashSet<string> _linearFusionSynergy = new HashSet<string>
        {
            "Linear Fusion",
            "Power",
            "Precision",
            "Rifle",
            "Heavy Lifting"
        };

        private static readonly HashSet<string> _machineGunSynergy = new HashSet<string>
        {
            "Machine Gun",
            "Power",
            "Heavy Lifting"
        };

        private static readonly Dictionary<string, HashSet<string>> _weaponTypeSynergies = new Dictionary<string, HashSet<string>>
        {
            { "Auto Rifle", _autoRifleSynergy },
            { "Scout Rifle", _scoutRifleSynergy },
            { "Pulse Rifle", _pulseRifleSynergy },
            { "Hand Cannon", _handCannonSynergy },
            { "Submachine Gun", _subMachineGunSynergy },
            { "Sidearm", _sidearmSynergy },
            { "Bow", _bowSynergy },
            { "Arrow", _bowSynergy },
            { "Shotgun", _shotgunSynergy },
            { "Grenade Launcher", _grenadeLauncherSynergy },
            { "Fusion Rifle", _fusionRifleSynergy },
            { "Sniper Rifle", _sniperRifleSynergy },
            { "Trace Rifle", _traceRifleSynergy },
            { "Sword", _swordSynergy },
            { "Rocket Launcher", _rocketLauncherSynergy },
            { "Linear Fusion", _linearFusionSynergy },
            { "Linear Fusion Rifle", _linearFusionSynergy },
            { "Machine Gun", _machineGunSynergy }
        };

        private static readonly HashSet<string> _primarySynergy = new HashSet<string>
        {
            "Auto Rifle",
            "Scout Rifle",
            "Pulse Rifle",
            "Hand Cannon",
            "Submachine Gun",
            "Sidearm",
            "Bow",
            "Light Arms",
            "Rifle",
            "Scatter",
            "Oversize",
            "Kinetic",
            "Energy",
        };

        private static readonly HashSet<string> _specialSynergy = new HashSet<string>
        {
            "Shotgun",
            "Grenade Launcher",
            "Fusion Rifle",
            "Sniper Rifle",
            "Trace Rifle",
            "Kinetic",
            "Energy",
            "Oversize",
            "Precision",
            "Scatter",
            "Rifle",
            "Light Reactor",
            "Pump Action",
            "Remote Connection",
        };

        private static readonly HashSet<string> _heavySynergy = new HashSet<string>
        {
            "Sword",
            "Grenade Launcher",
            "Rocket Launcher",
            "Linear Fusion",
            "Machine Gun",
            "Sniper Rifle",
            "Shotgun",
            "Power",
            "Oversize",
            "Rifle",
            "Heavy Lifting"
        };


        private static readonly Dictionary<string, HashSet<string>> _ammoFinderSynergies = new Dictionary<string, HashSet<string>>
        {
            { "Primary", _primarySynergy },
            { "Special", _specialSynergy },
            { "Heavy", _heavySynergy },
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

        private string CalculateAmmoFinderSynergy(int secondaryPerkIndex, string secondaryPerk, int primaryPerkIndex, string primaryPerk)
        {
            var sb = new StringBuilder();

            string weaponClass = ExtractWeaponClass(secondaryPerk);
            sb.Append(CalculateSecondaryPerkSynergy(secondaryPerkIndex, primaryPerkIndex, primaryPerk, weaponClass, _ammoFinderSynergies));

            return sb.ToString();
        }

        private string CalculateScavengerOrReservesSynergy(int secondaryPerkIndex, string secondaryPerk, int primaryPerkIndex, string primaryPerk)
        {
            var sb = new StringBuilder();

            string weaponName = ExtractWeaponName(secondaryPerk);
            sb.Append(CalculateSecondaryPerkSynergy(secondaryPerkIndex, primaryPerkIndex, primaryPerk, weaponName, _weaponTypeSynergies));

            return sb.ToString();
        }

        private string CalculateSecondaryPerkSynergy(int secondaryPerkIndex, string secondaryPerk, int primaryPerkIndex, string primaryPerk)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(primaryPerk))
            {
                if (secondaryPerk.Contains("Finder"))
                {
                    sb.Append(CalculateAmmoFinderSynergy(secondaryPerkIndex, secondaryPerk, primaryPerkIndex, primaryPerk));
                }
                else // Must be Scavanger or Reserves perk
                {
                    sb.Append(CalculateScavengerOrReservesSynergy(secondaryPerkIndex, secondaryPerk, primaryPerkIndex, primaryPerk));
                }
            }

            return sb.ToString();
        }

        private string CalculateSecondaryPerkSynergy(int secondaryPerkIndex, int primaryPerkIndex, string primaryPerk, string key, Dictionary<string, HashSet<string>> dictionary)
        {
            var sb = new StringBuilder();

            primaryPerk = MassagePrimaryPerk(primaryPerk);

            HashSet<string> synergies = null;

            if (dictionary.TryGetValue(key, out synergies))
            {
                foreach (string synergy in synergies)
                {
                    if (primaryPerk.StartsWith(synergy))
                    {
                        sb.Append($"{primaryPerkIndex}_{secondaryPerkIndex}_");
                        break;
                    }
                }
            }

            return sb.ToString();
        }

        private string CalculateSynergy(PerkGroup primaryPerks, PerkGroup secondaryPerks)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(secondaryPerks.Perk1))
            {
                sb.Append(CalculateSecondaryPerkSynergy(1, secondaryPerks.Perk1, 1, primaryPerks.Perk1));
                sb.Append(CalculateSecondaryPerkSynergy(1, secondaryPerks.Perk1, 2, primaryPerks.Perk2));
                sb.Append(CalculateSecondaryPerkSynergy(1, secondaryPerks.Perk1, 3, primaryPerks.Perk3));
            }

            if (!string.IsNullOrWhiteSpace(secondaryPerks.Perk2))
            {
                sb.Append(CalculateSecondaryPerkSynergy(2, secondaryPerks.Perk2, 1, primaryPerks.Perk1));
                sb.Append(CalculateSecondaryPerkSynergy(2, secondaryPerks.Perk2, 2, primaryPerks.Perk2));
                sb.Append(CalculateSecondaryPerkSynergy(2, secondaryPerks.Perk2, 3, primaryPerks.Perk3));
            }

            return sb.ToString().Trim('_');
        }

        private string ExtractWeaponClass(string secondaryPerk)
        {
            return secondaryPerk.Substring(0, secondaryPerk.IndexOf("Ammo Finder"));
        }

        private string ExtractWeaponName(string secondaryPerk)
        {
            return secondaryPerk.Substring(0, secondaryPerk.LastIndexOf(' '));
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

        private string MassagePrimaryPerk(string primaryPerk)
        {
            if (primaryPerk.StartsWith(_unflinchingAimPrefix))
            {
                primaryPerk = primaryPerk.Substring(_unflinchingAimPrefix.Length);
            }
            else if (primaryPerk.StartsWith(_enhancedPrefix))
            {
                primaryPerk = primaryPerk.Substring(_enhancedPrefix.Length);
            }

            return primaryPerk;
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
