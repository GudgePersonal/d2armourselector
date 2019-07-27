// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using Interfaces;
    using System.Collections.Generic;
    using System.Text;

    class SimpleSynergyCalculator : ISynergyCalculator
    {
        public static ISynergyCalculator Instance = new SimpleSynergyCalculator();

        private static readonly string _enhancedPrefix = "Enhanced ";
        private static readonly string _unflinchingAimPrefix = "Unflinching ";

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
            "Oversize",
            "Large"
        };

        private static readonly HashSet<string> _grenadeLauncherSynergy = new HashSet<string>
        {
            "Grenade Launcher",
            "Kinetic",
            "Energy",
            "Power",
            "Oversize",
            "Large",
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
            "Large",
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

        public string CalculateSynergy(PerkGroup primaryPerks, PerkGroup secondaryPerks)
        {
            return CalculateSynergyInternal(primaryPerks, secondaryPerks);
        }

        private static string CalculateAmmoFinderSynergy(int secondaryPerkIndex, string secondaryPerk, int primaryPerkIndex, string primaryPerk)
        {
            var sb = new StringBuilder();

            string weaponClass = ExtractWeaponClass(secondaryPerk);
            sb.Append(CalculateSecondaryPerkSynergy(secondaryPerkIndex, primaryPerkIndex, primaryPerk, weaponClass, _ammoFinderSynergies));

            return sb.ToString();
        }

        private static string CalculateScavengerOrReservesSynergy(int secondaryPerkIndex, string secondaryPerk, int primaryPerkIndex, string primaryPerk)
        {
            var sb = new StringBuilder();

            string weaponName = ExtractWeaponName(secondaryPerk);
            sb.Append(CalculateSecondaryPerkSynergy(secondaryPerkIndex, primaryPerkIndex, primaryPerk, weaponName, _weaponTypeSynergies));

            return sb.ToString();
        }

        private static string CalculateSecondaryPerkSynergy(int secondaryPerkIndex, string secondaryPerk, int primaryPerkIndex, string primaryPerk)
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

        private static string CalculateSecondaryPerkSynergy(int secondaryPerkIndex, int primaryPerkIndex, string primaryPerk, string key, Dictionary<string, HashSet<string>> dictionary)
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

        private static string CalculateSynergyInternal(PerkGroup primaryPerks, PerkGroup secondaryPerks)
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

        private static string ExtractWeaponClass(string secondaryPerk)
        {
            return secondaryPerk.Substring(0, secondaryPerk.IndexOf("Ammo Finder")).Trim();
        }

        private static string ExtractWeaponName(string secondaryPerk)
        {
            return secondaryPerk.Substring(0, secondaryPerk.LastIndexOf(' ')).Trim();
        }

        private static string MassagePrimaryPerk(string primaryPerk)
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
    }
}
