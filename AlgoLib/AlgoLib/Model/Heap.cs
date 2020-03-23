using System.Collections.Generic;

namespace AlgoLib.Model
{
    public class Heap
    {
        #region Fields

        private List<int> _heapArray;
        private HeapType _heapType;

        #endregion

        #region Properties

        public int Root
        {
            get => _heapArray[0];
        }

        public int Size
        {
            get => _heapArray.Count;
        }

        public HeapType Type
        {
            get => _heapType;
        }
        
        #endregion

        #region Constructors

        public Heap(int Size,HeapType Type)
        {
            _heapArray = new List<int>(Size);
            _heapType = Type;
        }

        public Heap(HeapType Type)
        {
            _heapArray = new List<int>();
            _heapType = Type;
        }

        #endregion

        #region Methodes

        public int LeftChildIndex(int ParentNodeIndex)
        {
            CheckIndexBound(ParentNodeIndex);
            return ParentNodeIndex * 2;
        }

        public int RightChildIndex(int ParentNodeIndex) => LeftChildIndex(ParentNodeIndex) + 1;

        public int ParentOfNode(int ChildNodeIndex)
        {
            CheckIndexBound(ChildNodeIndex);
            return ChildNodeIndex / 2;
        }

        public int this[int Index] {
            get
            {
                CheckIndexBound(Index);
                return _heapArray[Index];
            }
        }

        public void Heapify(int RootIndex)
        {
            int Largest;
            int LeftIndex = LeftChildIndex(RootIndex);
            int RightIndex = RightChildIndex(RootIndex);
            if (LeftIndex < _heapArray.Count && Compare(LeftIndex,RootIndex))
                Largest = LeftIndex;
            else
                Largest = RootIndex;
            if (RightIndex < _heapArray.Count && Compare(RightIndex,Largest))
                Largest = RightIndex;
            if (Largest != RootIndex)
            {
                SwapNodes(RootIndex, Largest);
                Heapify(Largest);
            }
        }

        private void SwapNodes(int FirstNodeIndex, int SecondNodeIndex)
        {
            int Temp = _heapArray[FirstNodeIndex];
            _heapArray[FirstNodeIndex] = _heapArray[SecondNodeIndex];
            _heapArray[SecondNodeIndex] = Temp;
        }

        private void CheckIndexBound(int Index)
        {
            if (Index < 0 || Index > _heapArray.Count)
                throw new System.Exception("Index out of bounds");
        }

        public int PopRoot()
        {
            int result = Root;
            _heapArray.RemoveAt(0);
            return result;
        }

        private bool Compare(int FirstIndex, int SecondIndex)
        {
            switch (_heapType)
            {
                case HeapType.MaxHeap:
                    return _heapArray[FirstIndex] > _heapArray[SecondIndex];
                case HeapType.MinHeap:
                    return _heapArray[FirstIndex] < _heapArray[SecondIndex];
                default:
                    throw new System.Exception("Heap type is not set to a valid value!");
            }
        }

        #endregion

        #region Static Methodes

        public static Heap BuildHeap(int[] Array,HeapType Type)
        {
            Heap resultHeap = new Heap(Array.Length,Type);
            resultHeap._heapArray = new List<int>(CopyArray(Array));
            for (int i = Array.Length / 2; i >= 0; i--)
                resultHeap.Heapify(i);
            return resultHeap;
        }

        public static int[] CopyArray(int[] ArrayToCopy)
        {
            int[] resultArray = new int[ArrayToCopy.Length];
            ArrayToCopy.CopyTo(resultArray, 0);
            return resultArray;
        }

        #endregion

    }
}
