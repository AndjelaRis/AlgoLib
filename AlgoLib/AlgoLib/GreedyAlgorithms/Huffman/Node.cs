using System.Collections.Generic;

namespace AlgoLib.GreedyAlgorithms
{
    public class Node
    {
        public char Symbol { get; set; }
        public int Frequency { get; set; }
        public Node Right { get; set; }
        public Node Left { get; set; }

        public List<bool> Traverse(char symbol, List<bool> data)
        {
            if (Right == null && Left == null)
                if (symbol.Equals(this.Symbol))
                    return data;
                else
                    return null;
            List<bool> left = null;
            List<bool> right = null;
            if (Left != null)
                left = Left.Traverse(symbol, GetSubList(data, false));
            if (Right != null)
                right = Right.Traverse(symbol, GetSubList(data, true));
            if (left != null)
                return left;
            else
                return right;
        }

        public List<bool> GetSubList(List<bool> currentData, bool nextStep)
        {
            List<bool> rightPath = new List<bool>();
            rightPath.AddRange(currentData);
            rightPath.Add(nextStep);
            return rightPath;
        }

        public bool IsLeaf() => (Left == null && Right == null);
    }
}
