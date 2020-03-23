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
            // TestForBasicAlgorithms();
            SortingPerformanceCalculator.TempTestForNElements(10, SortDirection.Increasing);
        }

        #region Test cases

        public static void TempTestForAll()
        {
            SortingPerformanceCalculator.TempTestForNElements(10, SortDirection.Increasing);
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
