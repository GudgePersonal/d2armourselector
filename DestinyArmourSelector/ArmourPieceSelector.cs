// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class ArmourPieceSelector
    {
        public (IEnumerable<ArmourPiece> tokeep, IEnumerable<ArmourPiece> toDelete) ProcessArmourPieces(IList<ArmourPiece> pieces, CharacterClass characterClass)
        {
            IList<ArmourPiece> armourPieces = pieces.Where(x => x.Class == characterClass).ToList();

            return ProcessArmourPiecesInternal(armourPieces);
        }

        private (IEnumerable<ArmourPiece> tokeep, IEnumerable<ArmourPiece> toDelete) ProcessArmourPiecesInternal(IList<ArmourPiece> pieces)
        {
            HashSet<ArmourPiece> keepers = new HashSet<ArmourPiece>();
            HashSet<ArmourPiece> deleters = new HashSet<ArmourPiece>();
            HashSet<string> itemNamesUsed = new HashSet<string>();

            pieces = pieces.
                OrderByDescending(x => x.Synergy.Length).
                ThenByDescending(x => x.PowerLevel).
                ToList();

            foreach (ArmourPiece piece in pieces)
            {
                if (!itemNamesUsed.Contains(piece.Name))
                {
                    keepers.Add(piece);
                    itemNamesUsed.Add(piece.Name);
                }
            }

            // Remove keepers from pieces
            pieces = pieces.Where(x => !keepers.Contains(x)).ToList();

            HashSet<string> perksFound = new HashSet<string>();

            foreach (ArmourPiece piece in keepers)
            {
                if (ShouldInclude(piece)) // Don't add perks from exotics or low light items into the pool
                {
                    perksFound.Add(piece.PrimaryPerks.Perk1);
                    perksFound.Add(piece.PrimaryPerks.Perk2);
                    perksFound.Add(piece.PrimaryPerks.Perk3);

                    perksFound.Add(piece.SecondaryPerks.Perk1);
                    perksFound.Add(piece.SecondaryPerks.Perk2);
                }
            }

            foreach (ArmourPiece piece in pieces)
            {
                if (perksFound.Contains(piece.PrimaryPerks.Perk1) &&
                   perksFound.Contains(piece.PrimaryPerks.Perk2) &&
                   perksFound.Contains(piece.PrimaryPerks.Perk3) &&
                   perksFound.Contains(piece.SecondaryPerks.Perk1) &&
                   perksFound.Contains(piece.SecondaryPerks.Perk2))
                {
                    deleters.Add(piece);
                }
                else
                {
                    keepers.Add(piece);

                    if (ShouldInclude(piece)) // Don't add perks from exotics or low light items into the pool
                    {
                        perksFound.Add(piece.PrimaryPerks.Perk1);
                        perksFound.Add(piece.PrimaryPerks.Perk2);
                        perksFound.Add(piece.PrimaryPerks.Perk3);

                        perksFound.Add(piece.SecondaryPerks.Perk1);
                        perksFound.Add(piece.SecondaryPerks.Perk2);
                    }
                }
            }

            return (keepers, deleters);
        }

        

        private bool ShouldInclude(ArmourPiece piece)
        {
            if (piece.Name.StartsWith("Memory Of Cayde "))
            {
                return false;
            }

            if (piece.MasterWorkLevel == 0)
            {
                return false;
            }

            if (piece.PowerLevel <= 10)
            {
                return false;
            }

            return true;
        }
    }
}
