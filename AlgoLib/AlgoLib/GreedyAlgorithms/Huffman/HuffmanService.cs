using System;
using System.Collections.Generic;
using System.IO;

namespace AlgoLib.GreedyAlgorithms
{
    public class HuffmanService : IService
    {
        private HuffmanTree tree;

        public void Encode(string sourcePath, string outputPath)
        {
            tree = new HuffmanTree();
            tree.Build(sourcePath);
            List<bool> encodedSource = Encode(sourcePath);
            byte[] encodedBytes = EncodeBytesForWriting(encodedSource).ToArray();
            File.WriteAllBytes(outputPath, encodedBytes);
        }

        private List<bool> Encode(string sourcePath)
        {
            List<bool> encodedSource = new List<bool>();
            using (StreamReader fileReader = new StreamReader(sourcePath))
            {
                int nextCharIntValue = fileReader.Read();
                while (nextCharIntValue != -1)
                {
                    char nextChar = (char)nextCharIntValue;
                    List<bool> encodedSymbol = tree.Root.Traverse(nextChar, new List<bool>());
                    encodedSource.AddRange(encodedSymbol);
                    nextCharIntValue = fileReader.Read();
                }
            }
            return encodedSource;
        }

        public void Decode(string inputPath, string outputPath)
        {
            List<bool> fileInBytes = DecodeBytesForReading(inputPath);
            Decode(outputPath, fileInBytes);
        }

        private void Decode(string outputPath, List<bool> bytesInFile)
        {
            Node current = tree.Root;
            using (StreamWriter writer = new StreamWriter(outputPath, false))
                foreach (bool bit in bytesInFile)
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
                        current = tree.Root;
                    }
                }
        }

        private List<byte> EncodeBytesForWriting(List<bool> bitsToWrite)
        {
            List<byte> bytesToWrite = new List<byte>(bitsToWrite.Count / 8 + (bitsToWrite.Count % 8 == 0 ? 0 : 1));
            for (int i = 0; i < bytesToWrite.Capacity; i++)
            {
                uint value = 0;
                int j = 0;
                while (j < 8 && (i * 8 + j) < bitsToWrite.Count)
                {
                    if (bitsToWrite[i * 8 + j])
                        value += (uint)Math.Pow(2, j);
                    j++;
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

        public void PrintCodeTable()
        {
            this.tree.PrintTable();
        }
    }
}
