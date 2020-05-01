using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlgoLib.GreedyAlgorithms
{
    public class HuffmanTree
    {
        private readonly List<Node> _nodes;

        public Node Root { get; private set; }

        public HuffmanTree()
        {
            _nodes = new List<Node>();
            Root = null;
        }

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

        public void Build(string path)
        {
            Dictionary<char, int> frequencies = CreateFrequencyTable(path);
            foreach (KeyValuePair<char, int> symbol in frequencies)
                _nodes.Add(new Node() { Symbol = symbol.Key, Frequency = symbol.Value });
            while (_nodes.Count > 1)
            {
                List<Node> orderedNodes = _nodes.OrderBy(node => node.Frequency).ToList<Node>();
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
                    _nodes.Remove(taken[0]);
                    _nodes.Remove(taken[1]);
                    _nodes.Add(parent);
                }
                this.Root = _nodes.FirstOrDefault();
            }
        }

        public void PrintTable()
        {
            for (int i = 0; i < 256; i++)
            {
                List<bool> toShow = this.Root.Traverse((char)i, new List<bool>());
                Console.Write("Symbol {0} code: ", (char)i);
                foreach (bool singleBit in toShow)
                    Console.Write("{0}", singleBit ? 1 : 0);
                Console.WriteLine();
            }
        }
    }
}
