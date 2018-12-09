// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
                else if("-type".Equals(args[i], StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    _armourType = ArmourTypeHelpers.FromString(args[++i]);
                }
            }

            return ValidateCmdLine();
        }

        public async Task<bool> Run()
        {
            ArmourPieceFactory factory = ArmourPieceFactory.Create(_armourType);
            IList<ArmourPiece> armourPieces = new List<ArmourPiece>();

            using (TextReader reader = new StreamReader(_inputFileName))
            {
                var csvReader = new CsvReader(reader);

                IEnumerable<string> headerTokens = await csvReader.Read();
                IEnumerable<string> tokens;
                int i = 1;

                do
                {
                    tokens = await csvReader.Read();

                    if (tokens.Any())
                    {
                        ArmourPiece armourPiece = factory.CreateArmourPiece(++i, tokens);
                        armourPieces.Add(armourPiece);
                    }
                }
                while (tokens != null && tokens.Any());
            }

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
