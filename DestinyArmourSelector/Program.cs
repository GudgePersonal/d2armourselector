// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    class Program
    {
        private string _inputFileName = string.Empty;
        private ArmourType _armourType = ArmourType.Unknown;
        private bool _isDimInputFile = true;
        private bool _sortResult = true;
        private bool _sortByPowerLevel = true;

        static void Main(string[] args)
        {
            var program = new Program();

            if (program.ProcessCmdLine(args))
            {
                program.Run().GetAwaiter().GetResult();
            }
            else
            {
                Help();
            }
        }

        public static void Help()
        {
        }

        public bool ProcessCmdLine(string[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                if ("-inputFile".Equals(args[i], StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    _inputFileName = args[++i];
                }
                else if ("-type".Equals(args[i], StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    _armourType = ArmourTypeHelpers.FromString(args[++i]);
                }
                else if ("-notdim".Equals(args[i], StringComparison.OrdinalIgnoreCase))
                {
                    _isDimInputFile = false;
                }
                else if ("-nosort".Equals(args[i], StringComparison.OrdinalIgnoreCase))
                {
                    _sortResult = false;
                }
                else if ("-sortbyrow".Equals(args[i], StringComparison.OrdinalIgnoreCase))
                {
                    _sortByPowerLevel = false;
                }
            }

            return ValidateCmdLine();
        }

        public async Task<bool> Run()
        {
            IArmourPieceFactory factory = _isDimInputFile ? DIMArmourPieceFactory.Create(_armourType) : ArmourPieceFactory.Create(_armourType);
            var creator = new ArmourPieceCreator(_inputFileName, factory);

            IList<ArmourPiece> armourPieces = await creator.CreateArmourPieces();

            var titanSelector = new ArmourPieceSelector(armourPieces, CharacterClass.Titan);
            (IEnumerable<ArmourPiece> toKeep, IEnumerable<ArmourPiece> toDelete) titanPieces = titanSelector.ProcessArmourPieces();
            OutputResults(titanPieces.toKeep, titanPieces.toDelete);

            var hunterSelector = new ArmourPieceSelector(armourPieces, CharacterClass.Hunter);
            (IEnumerable<ArmourPiece> toKeep, IEnumerable<ArmourPiece> toDelete) hunterPieces = hunterSelector.ProcessArmourPieces();
            OutputResults(hunterPieces.toKeep, hunterPieces.toDelete);

            var warlockSelector = new ArmourPieceSelector(armourPieces, CharacterClass.Warlock);
            (IEnumerable<ArmourPiece> toKeep, IEnumerable<ArmourPiece> toDelete) warlockPieces = warlockSelector.ProcessArmourPieces();
            OutputResults(warlockPieces.toKeep, warlockPieces.toDelete);

            return true;
        }

        private bool OutputResults(IEnumerable<ArmourPiece> keep, IEnumerable<ArmourPiece> delete)
        {
            if (_sortResult)
            {
                keep = keep.OrderByDescending(x => _sortByPowerLevel ? x.PowerLevel : x.RowNumber);
                delete = delete.OrderByDescending(x => _sortByPowerLevel ? x.PowerLevel : x.RowNumber);
            }

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

        private bool ValidateCmdLine()
        {
            return _armourType != ArmourType.Unknown && !String.IsNullOrWhiteSpace(_inputFileName);
        }
    }
}
