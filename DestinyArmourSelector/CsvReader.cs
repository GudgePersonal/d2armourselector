namespace DestinyArmourSelector
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    class CsvReader
    {
        private static string[] _empty = new string[] { };
        private TextReader _reader;

        public CsvReader(TextReader reader)
        {
            _reader = reader;
        }

        public async Task<IEnumerable<string>> Read()
        {
            string line = await _reader.ReadLineAsync();

            if (line != null)
            {
                return line.Split(',');
            }
            else
            {
                return _empty;
            }
        }
    }
}
