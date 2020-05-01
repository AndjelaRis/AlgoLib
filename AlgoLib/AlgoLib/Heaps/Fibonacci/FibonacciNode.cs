namespace AlgoLib.Heaps.Fibonacci
{
    public class FibonacciNode
    {
        public FibonacciNode Parent { get; set; }
        public FibonacciNode Child { get; set; }
        public FibonacciNode Left { get; set; }
        public FibonacciNode Right { get; set; }
        public int Key { get; set; }
        public int Degree { get; set; }
        public bool IsMarked { get; set; }

        public FibonacciNode(int key)
        {
            Key = key;
            Parent = null;
            Child = null;
            Left = null;
            Right = null;
            Degree = 0;
            IsMarked = false;
        }
    }
}
