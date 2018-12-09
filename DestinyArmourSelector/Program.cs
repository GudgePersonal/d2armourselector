// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    class Program
    {
        string _inputFileName = string.Empty;
        ArmourType _armourType = ArmourType.Unknown;

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
            }

            return ValidateCmdLine();
        }

        public async Task<bool> Run()
        {
            var creator = new ArmourPieceCreator(_inputFileName, _armourType);

            ArmourPieceFactory factory = ArmourPieceFactory.Create(_armourType);
            IList<ArmourPiece> armourPieces = await creator.CreateArmourPieces();

            var selector = new ArmourPieceSelector();
            selector.ProcessArmourPieces(armourPieces);

            return true;
        }

        private bool ValidateCmdLine()
        {
            return _armourType != ArmourType.Unknown && !String.IsNullOrWhiteSpace(_inputFileName);
        }
    }
}
