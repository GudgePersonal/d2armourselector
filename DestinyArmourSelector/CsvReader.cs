﻿// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    using System.IO;
    using System.Threading.Tasks;

    class CsvReader
    {
        private static readonly string[] _empty = new string[] { };
        private TextReader _reader;

        public CsvReader(TextReader reader)
        {
            _reader = reader;
        }

        public async Task<string[]> Read()
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
