using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlgoLib.GreedyAlgorithms.ShannonFano
{
    public class ShannonFannoTree
    {
        private List<Node> nodes;

        public Node Root { get; private set; }

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

        private int SumOfList(List<Node> listOfNodes, int startIndex, int count)
        {
            int sumOfList = 0;
            for (int i = 0; i < count; i++)
                sumOfList += listOfNodes[i + startIndex].Frequency;
            return sumOfList;
        }

        private int ReturnDivider(List<Node> listOfNodes, int startIndex, int count)
        {
            if (listOfNodes.Count == 1  || count == 1)
                return startIndex;
            int rightSum = SumOfList(listOfNodes, startIndex, count)-listOfNodes[startIndex].Frequency;
            int leftSum = listOfNodes[startIndex].Frequency;
            int divider = startIndex;
            double ratio = Math.Abs(1 - (rightSum / (double)leftSum));
            while (divider < startIndex + count - 1)
            {
                int tempLeftSum = leftSum + listOfNodes[divider + 1].Frequency;
                int tempRightSum = rightSum - listOfNodes[divider + 1].Frequency;
                double tempRatio = Math.Abs(1 - tempLeftSum / (double)tempRightSum);
                if (ratio<tempRatio)
                    break;
                leftSum = tempLeftSum;
                rightSum = tempRightSum;
                ratio = tempRatio;
                divider++;
            }
            return divider;
        }

        public Node Divide(List<Node> listOfNodes, int startIndex, int count)
        {
            int divider = ReturnDivider(listOfNodes, startIndex, count);
            if (count == 1)
                return new Node
                {
                    Frequency = listOfNodes[startIndex].Frequency,
                    Left = null,
                    Right = null,
                    Symbol = listOfNodes[startIndex].Symbol
                };
            Node left = Divide(listOfNodes, startIndex, divider - startIndex +1);
            Node right = Divide(listOfNodes, divider + 1, (startIndex + count) - divider - 1);
            return new Node
            {
                Frequency = 0,
                Left = left,
                Right = right,
                Symbol = '*'
            };
        }

        public void BuildTree(string path)
        {
            Dictionary<char, int> table = CreateFrequencyTable(path);
            List<Node> listOfOrderedNodes = new List<Node>();
            foreach (KeyValuePair<char, int> symbol in table)
                listOfOrderedNodes.Add(new Node() { Symbol = symbol.Key, Frequency = symbol.Value });
            listOfOrderedNodes = listOfOrderedNodes.OrderByDescending(node => node.Frequency).ToList();
            Root = Divide(listOfOrderedNodes, 0, listOfOrderedNodes.Count);
        }

        public void Encode(string sourcePath, string outputPath)
        {
            List<bool> encodedSource = new List<bool>();
            using (StreamReader fileReader = new StreamReader(sourcePath))
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
            using (StreamWriter writer = new StreamWriter(outputPath, false))
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
