// Copyright Small & Fast 2018


namespace DestinyArmourSelector
{
    using System.Collections.Generic;
    using System.Linq;

    class ArmourPieceSelector
    {
        private readonly CharacterClass _characterClass = CharacterClass.Unknown;
        private HashSet<string> _perksFound = new HashSet<string>();
        private IEnumerable<ArmourPiece> _pieces;
        private bool _reverse;
        

        public ArmourPieceSelector(IEnumerable<ArmourPiece> pieces, CharacterClass characterClass, bool reverse)
        {            
            _characterClass = characterClass;
            _pieces = pieces;
            _reverse = reverse;
        }

        public (IEnumerable<ArmourPiece> toKeep, IEnumerable<ArmourPiece> toDelete) ProcessArmourPieces()
        {
            IEnumerable<ArmourPiece> armourPieces = _pieces.Where(x => x.Class == _characterClass);
            return ProcessArmourPiecesInternal(armourPieces);
        }

        private (IEnumerable<ArmourPiece> toKeep, IEnumerable<ArmourPiece> toDelete) ProcessArmourPiecesInternal(IEnumerable<ArmourPiece> pieces)
        {
            var toKeep = new List<ArmourPiece>();
            var toDelete = new List<ArmourPiece>();

            //pieces = pieces.OrderByDescending(x => x.Synergy.Length).ThenByDescending(x => x.PowerLevel);
            //pieces = pieces.OrderByDescending(x => x.Synergy.Length);
            //pieces = pieces.OrderByDescending(x => x.PowerLevel);
            pieces = pieces.OrderBy(x => x.Synergy.Length).ThenBy(x => x.PowerLevel);
            //pieces = pieces.OrderBy(x => x.Synergy.Length);
            //pieces = pieces.OrderBy(x => x.PowerLevel);

            UpdateKeepAndDeleteLists(pieces, toKeep, toDelete);

            if (_reverse)
            {
                toKeep.Reverse();
                pieces = new List<ArmourPiece>(toKeep);
                toKeep.Clear();
                _perksFound.Clear();

                UpdateKeepAndDeleteLists(pieces, toKeep, toDelete);
            }

            return (toKeep, toDelete);
        }

        private void UpdateKeepAndDeleteLists(IEnumerable<ArmourPiece> pieces, List<ArmourPiece> toKeep, List<ArmourPiece> toDelete)
        {
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
