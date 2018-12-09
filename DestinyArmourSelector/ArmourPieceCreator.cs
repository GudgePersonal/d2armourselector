// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    class ArmourPieceCreator
    {
        private readonly ArmourType _armourType = ArmourType.Unknown;
        private readonly string _fileName = string.Empty;

        public ArmourPieceCreator(string fileName, ArmourType type)
        {
            _fileName = fileName;
            _armourType = type;
        }

        public async Task<IList<ArmourPiece>> CreateArmourPieces()
        {
            ArmourPieceFactory factory = ArmourPieceFactory.Create(_armourType);
            IList<ArmourPiece> armourPieces = new List<ArmourPiece>();

            using (TextReader reader = new StreamReader(_fileName))
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
                while (tokens.Any());
            }

            return armourPieces;
        }
    }
}
