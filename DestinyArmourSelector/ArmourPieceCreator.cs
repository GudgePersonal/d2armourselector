// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    class ArmourPieceCreator
    {
        private readonly string _fileName = string.Empty;
        private readonly IArmourPieceFactory _factory;

        public ArmourPieceCreator(string fileName, IArmourPieceFactory factory)
        {
            _fileName = fileName;
            _factory = factory;
        }

        public async Task<IList<ArmourPiece>> CreateArmourPieces()
        {
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
                        ArmourPiece armourPiece = _factory.CreateArmourPiece(++i, tokens);

                        if (armourPiece != null)
                        {
                            armourPieces.Add(armourPiece);
                        }
                    }
                }
                while (tokens.Length > 0);
            }

            return armourPieces;
        }
    }
}
