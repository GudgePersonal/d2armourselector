// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System.Collections.Generic;
    using System.IO;
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
            IArmourPieceFactory factory = ArmourPieceFactory.Create(_armourType);
            IList<ArmourPiece> armourPieces = new List<ArmourPiece>();

            using (TextReader reader = new StreamReader(_fileName))
            {
                var csvReader = new CsvReader(reader);

                // Skip header row
                await csvReader.Read();

                string[] tokens;
                int i = 1;

                do
                {
                    tokens = await csvReader.Read();

                    if (tokens.Length > 0)
                    {
                        ArmourPiece armourPiece = factory.CreateArmourPiece(++i, tokens);
                        armourPieces.Add(armourPiece);
                    }
                }
                while (tokens.Length > 0);
            }

            return armourPieces;
        }
    }
}
