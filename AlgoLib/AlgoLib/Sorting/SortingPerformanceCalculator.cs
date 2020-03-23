using System;
using System.Diagnostics;
using AlgoLib.Model;

namespace AlgoLib.Sorting
{
    public class SortingPerformanceCalculator
    {
        private static int LowerLimit = 0;
        private static int UpperLimit = 10000;

        #region Static methodes for sorting performance

        public static PerformanceReport CalculateSortingPerformance(Func<int[],SortDirection, int[]> SortingAlgorithm, int[] UnorderedArray,SortDirection Direction)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int[] SortingResult = SortingAlgorithm(UnorderedArray,Direction);
            stopwatch.Stop();
            return new PerformanceReport
            {
                ElapsedTime = stopwatch.Elapsed,
                Result = SortingResult
            };
        }

        public static PerformanceReport CalculateSortingPerformance(Func<int[], int,SortDirection, int[]> SortingAlgorithm, int[] UnorderedArray, int BounderyInt, SortDirection Direction)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int[] SortingResult = SortingAlgorithm(UnorderedArray, BounderyInt, Direction);
            stopwatch.Stop();
            return new PerformanceReport
            {
                ElapsedTime = stopwatch.Elapsed,
                Result = SortingResult
            };
        }

        public static void PrintSortingReport(PerformanceReport Report)
        {
           foreach (int singleNumber in (int[])Report.Result)
                Console.Write("{0}\t", singleNumber);
            Console.WriteLine("Elapsed time: {0}\n", Report.ElapsedTime.TotalMilliseconds);
        }


        #endregion

        #region Testing functions for sorting algorithms

        public static void TestBasicForNElements(int N, SortDirection Direction)
        {
            int[] UnorderedArray = SortingPerformanceCalculator.GenerateRandomArray(LowerLimit, UpperLimit, N);
            
            Console.WriteLine("Insertion Sort");
            SortingPerformanceCalculator.PrintSortingReport(SortingPerformanceCalculator
                .CalculateSortingPerformance(SortingController.SelectionSort, UnorderedArray, Direction));

            Console.WriteLine("Selection Sort");
            SortingPerformanceCalculator.PrintSortingReport(SortingPerformanceCalculator
                .CalculateSortingPerformance(SortingController.HeapSort, UnorderedArray, Direction));

            Console.WriteLine("Bubble Sort");
            SortingPerformanceCalculator.PrintSortingReport(SortingPerformanceCalculator
                .CalculateSortingPerformance(SortingController.CountingSort, UnorderedArray, UpperLimit, Direction));
        }

        public static void TempTestForNElements(int N, SortDirection Direction)
        {
            int[] UnorderedArray = SortingPerformanceCalculator.GenerateRandomArray(LowerLimit, UpperLimit, N);

            Console.WriteLine("Counting sort");
            SortingPerformanceCalculator.PrintSortingReport(SortingPerformanceCalculator
                .CalculateSortingPerformance(SortingController.CountingSort, UnorderedArray, UpperLimit, Direction));

            Console.WriteLine("Radix sort");
            SortingPerformanceCalculator.PrintSortingReport(SortingPerformanceCalculator
                .CalculateSortingPerformance(SortingController.RadixSort, UnorderedArray, Direction));

        }

        #endregion

        public static int[] GenerateRandomArray(int LowerLimit,int UpperLimit,int NumberOfElements)
        {
            int[] resultArray = new int[NumberOfElements];
            Random RandomCreator = new Random();
            for (int i = 0; i < NumberOfElements; i++)
                resultArray[i] = RandomCreator.Next(LowerLimit, UpperLimit);
            return resultArray;
        }
    }
}
