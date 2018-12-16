// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System.Collections.Generic;
    using System.Linq;

    class ArmourPieceSelector
    {
        private HashSet<string> _perksFound = new HashSet<string>();
        private IEnumerable<ArmourPiece> _pieces;
        private readonly CharacterClass _characterClass = CharacterClass.Unknown;

        public ArmourPieceSelector(IEnumerable<ArmourPiece> pieces, CharacterClass characterClass)
        {
            _pieces = pieces;
            _characterClass = characterClass;
        }

        public (IEnumerable<ArmourPiece> toKeep, IEnumerable<ArmourPiece> toDelete) ProcessArmourPieces()
        {
            IList<ArmourPiece> armourPieces = _pieces.Where(x => x.Class == _characterClass).ToList();

            return ProcessArmourPiecesInternal(armourPieces);
        }

        private (IEnumerable<ArmourPiece> toKeep, IEnumerable<ArmourPiece> toDelete) ProcessArmourPiecesInternal(IList<ArmourPiece> pieces)
        {
            var toKeep = new List<ArmourPiece>();
            var toDelete = new List<ArmourPiece>();

            pieces = pieces.OrderBy(x => x.Name).ThenByDescending(x => x.PowerLevel).ToList();
            pieces = pieces.OrderByDescending(x => x.Synergy.Length).ThenByDescending(x => x.PowerLevel).ToList();

            foreach (ArmourPiece piece in pieces)
            {
                if (ShouldDelete(piece))
                {
                    toDelete.Add(piece);
                }
                else
                {
                    toKeep.Add(piece);

                    if (ShouldIncludePerks(piece)) // Don't add perks from exotics or low light items into the pool
                    {
                        AddPerks(piece);
                    }
                }
            }

            return (toKeep, toDelete);
        }

        private void AddPerks(ArmourPiece piece)
        {
            _perksFound.Add(piece.Name);

            _perksFound.Add(piece.PrimaryPerks.Perk1);
            _perksFound.Add(piece.PrimaryPerks.Perk2);
            _perksFound.Add(piece.PrimaryPerks.Perk3);

            _perksFound.Add(piece.SecondaryPerks.Perk1);
            _perksFound.Add(piece.SecondaryPerks.Perk2);
        }

        private bool ShouldDelete(ArmourPiece piece)
        {
            return
                _perksFound.Contains(piece.Name) &&
                _perksFound.Contains(piece.PrimaryPerks.Perk1) &&
                _perksFound.Contains(piece.PrimaryPerks.Perk2) &&
                _perksFound.Contains(piece.PrimaryPerks.Perk3) &&
                _perksFound.Contains(piece.SecondaryPerks.Perk1) &&
                _perksFound.Contains(piece.SecondaryPerks.Perk2);
        }

        private bool ShouldIncludePerks(ArmourPiece piece)
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
