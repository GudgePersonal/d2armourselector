﻿// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    public enum CharacterClass
    {
        Unknown,
        Hunter,
        Titan,
        Warlock
    }

    public static class CharacterClassHelpers
    {
        public static CharacterClass FromString(string s)
        {
            switch (s)
            {
            case "Hunter":
            case "hunter":
                return CharacterClass.Hunter;
            case "Titan":
            case "titan":
                return CharacterClass.Titan;
            case "Warlock":
            case "warlock":
                return CharacterClass.Warlock;
            default:
                return CharacterClass.Unknown;
            }
        }

        public static string ToString(CharacterClass c)
        {
            switch (c)
            {
            case CharacterClass.Hunter:
                return "Hunter";
            case CharacterClass.Titan:
                return "Titan";
            case CharacterClass.Warlock:
                return "Warlock";
            default:
                return "Unknown";
            }
        }
    }
}
