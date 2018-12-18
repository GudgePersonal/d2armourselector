// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    class Program
    {
        private string _inputFileName = string.Empty;
        private ArmourType _armourType = ArmourType.Unknown;
        private bool _isDimInputFile = true;

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
            }

            return ValidateCmdLine();
        }

        public async Task<bool> Run()
        {
            IArmourPieceFactory factory = _isDimInputFile ? DIMArmourPieceFactory.Create(_armourType) : ArmourPieceFactory.Create(_armourType);
            var creator = new ArmourPieceCreator(_inputFileName, factory);

            IList<ArmourPiece> armourPieces = await creator.CreateArmourPieces();

            var hunterSelector = new ArmourPieceSelector(armourPieces, CharacterClass.Hunter);
            (IEnumerable<ArmourPiece> toKeep, IEnumerable<ArmourPiece> toDelete) hunterPieces = hunterSelector.ProcessArmourPieces();
            OutputResults(hunterPieces.toKeep, hunterPieces.toDelete);

            var titanSelector = new ArmourPieceSelector(armourPieces, CharacterClass.Titan);
            (IEnumerable<ArmourPiece> toKeep, IEnumerable<ArmourPiece> toDelete) titanPieces = titanSelector.ProcessArmourPieces();
            OutputResults(titanPieces.toKeep, titanPieces.toDelete);

            var warlockSelector = new ArmourPieceSelector(armourPieces, CharacterClass.Warlock);
            (IEnumerable<ArmourPiece> toKeep, IEnumerable<ArmourPiece> toDelete) warlockPieces = warlockSelector.ProcessArmourPieces();
            OutputResults(warlockPieces.toKeep, warlockPieces.toDelete);

            return true;
        }

        private bool OutputResults(IEnumerable<ArmourPiece> keep, IEnumerable<ArmourPiece> delete)
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

        private bool ValidateCmdLine()
        {
            return _armourType != ArmourType.Unknown && !String.IsNullOrWhiteSpace(_inputFileName);
        }
    }
}
