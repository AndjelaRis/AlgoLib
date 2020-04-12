using AlgoLib.Sorting;
using AlgoLib.MatchingAlgorithms;

namespace TestProject
{
    class Program
    {
        //Ostala je implementacija:
        //Bucket
        //Radix
        static void Main(string[] args)
        {
            string text = @"E:\text10words.txt";
            string pattern = @"E:\pattern.txt";
            string fileOfIndexes = @"E:\fileOfIndexes.txt";
            //int primeNumber = 73;
            //int numOfChar = 256;
            //SearchAlgorithams.RabinKarp_Match(text, pattern, fileOfIndexes, numOfChar, primeNumber);
            //SearchAlgorithams.KnuthMorrisPratt_Match(text, pattern, fileOfIndexes);
            SearchAlgorithms.SoundEx(text, pattern, fileOfIndexes);


            //TestForBasicAlgorithms();
            //SortingPerformanceCalculator.TempTestForNElements(10, SortDirection.Increasing);
            TempTestForAll();
        }

        #region Test cases

        public static void TempTestForAll()
        {
            SortingPerformanceCalculator.TempTestForNElements(10, SortDirection.Increasing);
            //SortingPerformanceCalculator.TestBasicForNElements(10, SortDirection.Increasing);
            SortingPerformanceCalculator.TempTestForNElements(100, SortDirection.Increasing);
            SortingPerformanceCalculator.TempTestForNElements(1000, SortDirection.Increasing);
            SortingPerformanceCalculator.TempTestForNElements(10000, SortDirection.Increasing);
            SortingPerformanceCalculator.TempTestForNElements(100000, SortDirection.Increasing);
            SortingPerformanceCalculator.TempTestForNElements(1000000, SortDirection.Increasing);
        }

        public static void TestForBasicAlgorithms()
        {
            SortingPerformanceCalculator.TestBasicForNElements(100, SortDirection.Increasing);

            SortingPerformanceCalculator.TestBasicForNElements(1000, SortDirection.Increasing);

            SortingPerformanceCalculator.TestBasicForNElements(10000, SortDirection.Increasing);

            SortingPerformanceCalculator.TestBasicForNElements(100000, SortDirection.Increasing);
        }

        #endregion
    }
}
