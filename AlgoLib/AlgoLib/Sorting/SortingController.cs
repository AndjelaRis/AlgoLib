using AlgoLib.Model;
using System.Collections.Generic;

namespace AlgoLib.Sorting
{
    public class SortingController
    {
        #region Basic Algorithms

        public static int[] InsertionSort(int[] UnorderedArray, SortDirection Direction)
        {
            int[] sortedArray = CopyArray(UnorderedArray);
            for (int i = 1; i < sortedArray.Length; i++)
            {
                int key = sortedArray[i];
                int j = i - 1;
                while (j >= 0 && SortingController.Compare(sortedArray[j],key,Direction))
                    sortedArray[j + 1] = sortedArray[j--];
                sortedArray[j + 1] = key;
            }
            return sortedArray;
        }

        public static int[] SelectionSort(int[] UnorderedArray, SortDirection Direction)
        {
            int[] sortedArray = CopyArray(UnorderedArray);
            for (int i = 0; i < sortedArray.Length-1; i++)
            { 
                int minimumIndex = i;
                for (int j = i + 1; j < sortedArray.Length; j++)
                    if (Compare(sortedArray[minimumIndex],sortedArray[j],Direction))
                        minimumIndex = j;

                SwapElements(minimumIndex, i, sortedArray);
            }
            return sortedArray;
        }

        public static int[] BubbleSort(int[] UnorderedArray, SortDirection Direction)
        {
            int[] sortedArray = CopyArray(UnorderedArray);
            for (int i = 0; i < sortedArray.Length; i++)
                for (int j = 0; j < sortedArray.Length - i - 1; j++)
                    if (Compare(sortedArray[j], sortedArray[j + 1],Direction))
                        SwapElements(j, j + 1, sortedArray);
            return sortedArray;
        }

        #endregion


        #region Advanced Algorithms

        public static int[] HeapSort(int[] UnorderedArray, SortDirection Direction)
        {
            int[] sortedArray = new int[UnorderedArray.Length];
            Heap tempHeap;
            if (Direction == SortDirection.Increasing)
                tempHeap = Heap.BuildHeap(UnorderedArray, HeapType.MinHeap);
            else
                tempHeap = Heap.BuildHeap(UnorderedArray, HeapType.MaxHeap);
            int index = 0;
            while (tempHeap.Size > 0)
            {
                sortedArray[index++] = tempHeap.PopRoot();
                tempHeap.Heapify(0);
            }
            return sortedArray;
        }

        public static int[] CountingSort(int[] UnorderedArray, int BoundaryInt, SortDirection Direction)
        {
            int[] sortedArray = new int[UnorderedArray.Length];
            int[] tempArray = new int[BoundaryInt];
            int startingIndex, endingIndex, coeficient;
            if(Direction == SortDirection.Increasing)
            {
                startingIndex = 1;
                endingIndex = BoundaryInt-1;
                coeficient = 1;
            }
            else
            {
                startingIndex = BoundaryInt-2;
                endingIndex = 0;
                coeficient = -1;
            }
            
            for (int i = 0; i < tempArray.Length; i++)
                tempArray[i] = 0;
            for (int i = 0; i < UnorderedArray.Length; i++)
                tempArray[UnorderedArray[i]]++;
            for (int i = startingIndex; Compare(endingIndex,i,Direction); i+=1*coeficient)
                tempArray[i] += tempArray[i + 1*-coeficient];
            for (int i = UnorderedArray.Length - 1; i >= 0; i--)
            {
                sortedArray[tempArray[UnorderedArray[i]] - 1] = UnorderedArray[i];
                tempArray[UnorderedArray[i]]--;
            }
            return sortedArray;
        }

        public static int[] QuickSort(int[] UnorderedArray, SortDirection Direction)
        {
            int[] sortedArray = CopyArray(UnorderedArray);
            QuickSort(ref sortedArray,0,sortedArray.Length-1,Direction);
            return sortedArray;
        }

        public static int[] RadixSort(int[] UnorderedArray, SortDirection Direction)
        {
            int[] sortedArray = CopyArray(UnorderedArray);
            int max = GetMaxElement(UnorderedArray);
            for (int exponent = 1; max / exponent > 0; exponent *= 10)
                sortedArray = CountingSortWithExponent(sortedArray, exponent, Direction);
            return sortedArray;
        }

        #endregion


        #region Helper functions

        public static int[] CopyArray(int[] ArrayToCopy)
        {
            int[] resultArray = new int[ArrayToCopy.Length];
            ArrayToCopy.CopyTo(resultArray, 0);
            return resultArray;
        }

        public static void SwapElements(int FirstIndex, int SecondIndex, int[] Array)
        {
            int Temp = Array[FirstIndex];
            Array[FirstIndex] = Array[SecondIndex];
            Array[SecondIndex] = Temp;
        }

        public static bool Compare(int FirstInt, int SecondInt, SortDirection Direction)
        {
            switch (Direction)
            {
                case SortDirection.Increasing:
                    return FirstInt >= SecondInt;
                case SortDirection.Decreasing:
                    return FirstInt <= SecondInt;
                default:
                    throw new System.Exception("SortDirection is not set to a valid value!");
            }
        }

        private static void QuickSort(ref int[] UnorderedArray, int PivotIndex, int EndIndex, SortDirection Direction)
        {
            int newDivisionIndex;
            if (PivotIndex < EndIndex)
            {
                newDivisionIndex = Partition(ref UnorderedArray, PivotIndex, EndIndex, Direction);
                QuickSort(ref UnorderedArray, PivotIndex, newDivisionIndex - 1, Direction);
                QuickSort(ref UnorderedArray, newDivisionIndex + 1, EndIndex, Direction);
            }
        }

        private static int Partition(ref int[] UnorderedArray, int PivotIndex, int EndIndex, SortDirection Direction)
        {
            int key = UnorderedArray[EndIndex];
            int i = PivotIndex - 1;
            for(int j = PivotIndex;j<=EndIndex-1;j++)
                if (Compare(key, UnorderedArray[j], Direction))
                {
                    i++;
                    SwapElements(i, j, UnorderedArray);
                }
            SwapElements(i + 1, EndIndex,UnorderedArray);
            return i + 1;
        }

        private static int[] CountingSortWithExponent(int[] UnorderedArray, int Exponent, SortDirection Direction)
        {
            int[] sortedArray = new int[UnorderedArray.Length];
            int[] tempArray = new int[10];
            int startingIndex, endingIndex, coeficient;
            if (Direction == SortDirection.Increasing)
            {
                startingIndex = 1;
                endingIndex = 9;
                coeficient = 1;
            }
            else
            {
                startingIndex = 8;
                endingIndex = 0;
                coeficient = -1;
            }

            for (int i = 0; i < tempArray.Length; i++)
                tempArray[i] = 0;
            for (int i = 0; i < UnorderedArray.Length; i++)
                tempArray[(UnorderedArray[i]/Exponent)%10]++;
            for (int i = startingIndex; Compare(endingIndex,i,Direction); i += 1 * coeficient)
                tempArray[i] += tempArray[i + 1 * -coeficient];
            for (int i = UnorderedArray.Length - 1; i >= 0; i--)
            {
                int newIndex = tempArray[(UnorderedArray[i] / Exponent) % 10] - 1;
                sortedArray[newIndex] = UnorderedArray[i];
                tempArray[(UnorderedArray[i]/Exponent)%10]--;
            }
            return sortedArray;
        }
        
        public static int GetMaxElement(int[] Array)
        {
            int Max = Array[0];
            for (int i = 1; i < Array.Length; i++)
                if (Array[i] > Max)
                    Max = Array[i];
            return Max;
        }
        
        #endregion

    }
}
