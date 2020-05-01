using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgoLib.GreedyAlgorithms
{
    public class LzwService : IService
    {
        private Dictionary<string, uint> InitTable()
        {
            Dictionary<string, uint> table = new Dictionary<string, uint>();
            for (int i = char.MinValue; i < char.MaxValue; i++)
            {
                char c = Convert.ToChar(i);
                table.Add(c.ToString(),(uint)i);
            }
            return table;
        }

        private Dictionary<uint, string> InitInverseTable()
        {
            Dictionary<uint, string> table = new Dictionary<uint, string>();
            for (int i = char.MinValue; i < char.MaxValue; i++)
            {
                char c = Convert.ToChar(i);
                table.Add((uint)i, c.ToString());
            }
            return table;
        }

        private List<uint> EncodeToInt(string path)
        {
            Dictionary<string, uint> table = InitTable();
            string assembly = string.Empty;
            List<uint> compressed = new List<uint>();
            using(StreamReader fileReader = new StreamReader(path))
            {
                int intValueOfChar = fileReader.Read();
                while (intValueOfChar != -1)
                {
                    char nextChar = (char)intValueOfChar;
                    string nextAssembly = assembly + nextChar;
                    if (table.ContainsKey(nextAssembly))
                        assembly = nextAssembly;
                    else
                    {
                        compressed.Add(table[assembly]);
                        table.Add(nextAssembly, (uint)table.Count);
                        assembly = nextChar.ToString();
                    }
                    intValueOfChar = fileReader.Read();
                }
                if (!string.IsNullOrEmpty(assembly))
                    compressed.Add(table[assembly]);
            }
            return compressed;
        }

        private string DecodeFromInt(List<uint> encodedFile)
        {
            Dictionary<uint, string> table = InitInverseTable();
            string assembly = table[encodedFile[0]];
            encodedFile.RemoveAt(0);
            StringBuilder decompressed = new StringBuilder(assembly);
            foreach (uint singleInt in encodedFile)
            {
                string entry = null;
                if (table.ContainsKey(singleInt))
                    entry = table[singleInt];
                else if (singleInt == table.Count)
                    entry = assembly + assembly[0];
                decompressed.Append(entry);
                table.Add((uint)table.Keys.Max()+1, assembly + entry[0]);
                assembly = entry;
            }
            return decompressed.ToString();
        }

        public void Encode(string sourcePath, string outputPath)
        {
            List<uint> encodedFile = EncodeToInt(sourcePath);
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

        public void PrintTable(Dictionary<string, uint> codeTable)
        {
            foreach(KeyValuePair<string, uint> pair in codeTable)
                Console.WriteLine("Symbol {0} code: {1}", pair.Key,pair.Value);
        }
    }
}
