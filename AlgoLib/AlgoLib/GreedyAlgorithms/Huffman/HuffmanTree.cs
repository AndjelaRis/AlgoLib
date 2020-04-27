using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlgoLib.GreedyAlgorithms
{
    public class HuffmanTree
    {
        private List<Node> nodes = new List<Node>();
        public Node Root { get; set; }

        private Dictionary<char, int> CreateFrequencyTable(string path)
        {
            Dictionary<char, int> charMap = new Dictionary<char, int>();
            using (StreamReader fileReader = new StreamReader(path))
            {
                int nextCharIntValue = fileReader.Read();
                while (nextCharIntValue != -1)
                {
                    char nextChar = (char)nextCharIntValue;
                    if (charMap.ContainsKey(nextChar))
                        charMap[nextChar] += 1;
                    else
                        charMap.Add(nextChar, 1);
                    nextCharIntValue = fileReader.Read();
                }
            }
            return charMap;
        }

        private List<byte> EncodeBytesForWriting(List<bool> bitsToWrite)
        {
            List<byte> bytesToWrite = new List<byte>(bitsToWrite.Count / 8 + (bitsToWrite.Count % 8 == 0 ? 0 : 1));
            for (int i = 0; i < bytesToWrite.Capacity; i++)
            {
                uint value = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (bitsToWrite[i * 8 + j])
                        value += (uint)Math.Pow(2, j);
                }
                bytesToWrite.Add(Convert.ToByte(value));
            }
            return bytesToWrite;
        }

        private List<bool> DecodeBytesForReading(string inputPath)
        {
            List<bool> fileInBytes = new List<bool>();
            using (BinaryReader fileReader = new BinaryReader(new FileStream(inputPath, FileMode.Open)))
            {
                byte singleByte = fileReader.ReadByte();
                while (fileReader.BaseStream.Position != fileReader.BaseStream.Length)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        int mask = (int)Math.Pow(2, i);
                        int n = (int)singleByte;
                        bool singleBit = (n & mask) == mask;
                        fileInBytes.Add(singleBit);
                    }
                    singleByte = fileReader.ReadByte();
                }
            }
            return fileInBytes;
        }
        
        public void Build(string path)
        {
            Dictionary<char, int> frequencies = CreateFrequencyTable(path);
            foreach (KeyValuePair<char, int> symbol in frequencies)
                nodes.Add(new Node() { Symbol = symbol.Key, Frequency = symbol.Value });
            while (nodes.Count > 1)
            {
                List<Node> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList<Node>();
                if (orderedNodes.Count >= 2)
                {
                    List<Node> taken = orderedNodes.Take(2).ToList<Node>();
                    Node parent = new Node()
                    {
                        Symbol = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };
                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }
                this.Root = nodes.FirstOrDefault();
            }
        }

        public void Encode(string sourcePath, string outputPath)
        {
            List<bool> encodedSource = new List<bool>();
            using(StreamReader fileReader = new StreamReader(sourcePath))
            {
                int nextCharIntValue = fileReader.Read();
                while (nextCharIntValue != -1)
                {
                    char nextChar = (char)nextCharIntValue;
                    List<bool> encodedSymbol = this.Root.Traverse(nextChar, new List<bool>());
                    encodedSource.AddRange(encodedSymbol);
                    nextCharIntValue = fileReader.Read();
                }
            }
            File.WriteAllBytes(outputPath, EncodeBytesForWriting(encodedSource).ToArray());
        }

        public void Decode(string inputPath, string outputPath)
        {
            List<bool> fileInBytes = DecodeBytesForReading(inputPath);
            Node current = this.Root;
            using(StreamWriter writer = new StreamWriter(outputPath, false))
                foreach (bool bit in fileInBytes)
                {
                    if (bit)
                    {
                        if (current.Right != null)
                            current = current.Right;
                    }
                    else
                        if (current.Left != null)
                            current = current.Left;
                    if (current.IsLeaf())
                    {
                        writer.Write(current.Symbol);
                        current = this.Root;
                    }
                }
        }

    }
}
