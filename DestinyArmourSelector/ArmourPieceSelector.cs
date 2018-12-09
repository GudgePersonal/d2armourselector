// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class ArmourPieceSelector
    {
        public bool ProcessArmourPieces(IList<ArmourPiece> pieces)
        {
            IList<ArmourPiece> hunterArmour = pieces.Where(x => x.Class == CharacterClass.Hunter).ToList();
            IList<ArmourPiece> titanArmour = pieces.Where(x => x.Class == CharacterClass.Titan).ToList();
            IList<ArmourPiece> warlockArmour = pieces.Where(x => x.Class == CharacterClass.Warlock).ToList();

            ProcessArmourPiecesInternal(hunterArmour);
            ProcessArmourPiecesInternal(titanArmour);
            ProcessArmourPiecesInternal(warlockArmour);

            return true;
        }

        private bool ProcessArmourPiecesInternal(IList<ArmourPiece> pieces)
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

            OutputKeepersAndDeleters(keepers, deleters);

            return true;
        }

        private bool OutputKeepersAndDeleters(IEnumerable<ArmourPiece> keep, IEnumerable<ArmourPiece> delete)
        {
            keep = keep.OrderBy(x => x.RowNumber);
            delete = delete.OrderBy(x => x.RowNumber);

            Console.WriteLine($"Keep: {keep.Count()}");
            Console.WriteLine();

            foreach (ArmourPiece piece in keep)
            {
                Console.WriteLine(piece.ToString());
            }

            Console.WriteLine();

            Console.WriteLine($"Delete: {delete.Count()}");
            Console.WriteLine();

            foreach (ArmourPiece piece in delete)
            {
                Console.WriteLine(piece.ToString());
            }

            Console.WriteLine();

            return true;
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
