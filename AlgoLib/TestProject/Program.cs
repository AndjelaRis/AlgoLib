using AlgoLib.Sorting;

namespace TestProject
{
    class Program
    {
        //Ostala je implementacija:
        //Bucket
        //Radix
        static void Main(string[] args)
        {
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
