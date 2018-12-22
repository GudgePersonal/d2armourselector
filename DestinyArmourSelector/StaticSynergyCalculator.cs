// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using Interfaces;

    public class StaticSynergyCalculator : ISynergyCalculator
    {
        private readonly string _synergy = string.Empty;

        public StaticSynergyCalculator(string synergy)
        {
            _synergy = synergy;
        }

        public string CalculateSynergy(PerkGroup primaryPerks, PerkGroup secondaryPerks)
        {
            return _synergy;
        }
    }
}
