using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgoLib.GreedyAlgorithms
{
    public class LZWService
    {

        public Dictionary<string, uint> InitTable()
        {
            Dictionary<string, uint> table = new Dictionary<string, uint>();
            for (ushort i = 0; i < 256; i++)
                table.Add(((char)i).ToString(), i);
            return table;
        }

        public Dictionary<uint, string> InitInverseTable()
        {
            Dictionary<uint, string> table = new Dictionary<uint, string>();
            for (ushort i = 0; i < 256; i++)
                table.Add(i, ((char)i).ToString());
            return table;
        }

        public LZWService()
        {
        }

        private List<uint> EncodeToInt(string path)
        {
            Dictionary<string, uint> table = InitTable();
            string w = string.Empty;
            List<uint> compressed = new List<uint>();
            using(StreamReader fileReader = new StreamReader(path))
            {
                int intValueOfChar = fileReader.Read();
                while (intValueOfChar != -1)
                {
                    char nextChar = (char)intValueOfChar;
                    string wc = w + nextChar;
                    if (table.ContainsKey(wc))
                        w = wc;
                    else
                    {
                        compressed.Add(table[w]);
                        table.Add(wc, (uint)table.Count);
                        w = nextChar.ToString();
                    }
                    intValueOfChar = fileReader.Read();
                }
                if (!string.IsNullOrEmpty(w))
                    compressed.Add(table[w]);
            }
            return compressed;
        }

        private string DecodeFromInt(List<uint> encodedFile)
        {
            Dictionary<uint, string> table = InitInverseTable();
            string w = table[encodedFile[0]];
            encodedFile.RemoveAt(0);
            StringBuilder decompressed = new StringBuilder(w);
            foreach (uint singleInt in encodedFile)
            {
                string entry = null;
                if (table.ContainsKey(singleInt))
                    entry = table[singleInt];
                else if (singleInt == table.Count)
                    entry = w + w[0];

                decompressed.Append(entry);

                table.Add((uint)table.Count, w + entry[0]);

                w = entry;
            }
            return decompressed.ToString();
        }

        public void Encode(string inputPath, string outputPath)
        {
            List<uint> encodedFile = EncodeToInt(inputPath);
            List<byte> bytesToWrite = new List<byte>(encodedFile.Count * 4);
            foreach (uint singleInt in encodedFile)
                bytesToWrite.AddRange(BitConverter.GetBytes(singleInt));
            File.WriteAllBytes(outputPath, bytesToWrite.ToArray());
        }

        public void Decode(string inputPath, string outputPath)
        {
            List<uint> codeFromFile = new List<uint>();
            using (BinaryReader binaryReader = new BinaryReader(new FileStream(inputPath, FileMode.Open)))
            {
                uint curentInt = binaryReader.ReadUInt32();
                while(binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                {
                    codeFromFile.Add(curentInt);
                    curentInt = binaryReader.ReadUInt32();
                }
            }
            File.WriteAllText(outputPath, DecodeFromInt(codeFromFile));
        }

    }
}
