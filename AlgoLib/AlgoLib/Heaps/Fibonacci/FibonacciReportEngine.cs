using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlgoLib.Heaps.Fibonacci
{
    public class FibonacciReportEngine
    {
        private readonly int _lowerLimit;
        private readonly int _maxLimit;

        public FibonacciReportEngine(int lowerLimit, int maxLimit)
        {
            _lowerLimit = lowerLimit;
            _maxLimit = maxLimit;
        }

        public List<FibonacciNode> TestInsertionForNElements(FibonacciHeap heap, int numOfInsertions)
        {
            List<int> keys = new List<int>(numOfInsertions);
            List<FibonacciNode> allNodes = new List<FibonacciNode>(numOfInsertions);
            Random generator = new Random();
            for (int i = 0; i < numOfInsertions; i++)
                keys.Add(generator.Next(_lowerLimit, _maxLimit));
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach(int singleKey in keys)
                allNodes.Add(heap.Insert(singleKey));
            stopwatch.Stop();
            Console.WriteLine($"Insertion of {numOfInsertions} elements done in {stopwatch.ElapsedMilliseconds} ms");
            return allNodes;
        }

        public void TestExtractForNElements(FibonacciHeap heap, int numOfExtractions)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < numOfExtractions; i++)
                heap.ExtractMin();
            stopwatch.Stop();
            Console.WriteLine($"Extraction of {numOfExtractions} minimum values done in {stopwatch.ElapsedMilliseconds} ms");
        }

        public void TestDecreaseKeyForNElements(FibonacciHeap heap, List<FibonacciNode> allNodes, int numOfDecreases)
        {
            Random generator = new Random();
            List<FibonacciNode> nodesToAlter = new List<FibonacciNode>();
            for (int i = 0; i < numOfDecreases; i++)
                nodesToAlter.Add(allNodes[generator.Next(0,allNodes.Count)]);
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach(FibonacciNode singleNode in nodesToAlter)
                heap.DecreaseKey(singleNode, generator.Next(0, singleNode.Key));
            stopwatch.Stop();
            Console.WriteLine($"Decrease of {numOfDecreases} keys done in {stopwatch.ElapsedMilliseconds} ms");
        }

        public void TestDeleteNodeForNElements(FibonacciHeap heap, List<FibonacciNode> allNodes, int numOfDeletions)
        {
            Random generator = new Random();
            List<int> nodesToDelete = new List<int>();
            for (int i = 0; i < numOfDeletions; i++)
            {
                int randomIndex = generator.Next(0, allNodes.Count);
                while (nodesToDelete.Exists(index => index == randomIndex))
                    randomIndex = generator.Next(0, allNodes.Count);
                nodesToDelete.Add(randomIndex);
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (int singleIndex in nodesToDelete)
                heap.Delete(allNodes[singleIndex]);
            stopwatch.Stop();
            Console.WriteLine($"Deletion of {numOfDeletions} nodes done in {stopwatch.ElapsedMilliseconds} ms");
        }

        public FibonacciHeap GenerateRandomHeap(int numOfElements, out List<FibonacciNode> allNodes)
        {
            List<int> keys = new List<int>(numOfElements);
            allNodes = new List<FibonacciNode>(numOfElements);
            FibonacciHeap newHeap = new FibonacciHeap();
            Random generator = new Random();
            for (int i = 0; i < numOfElements; i++)
                keys.Add(generator.Next(_lowerLimit, _maxLimit));
            foreach (int singleKey in keys)
                allNodes.Add(newHeap.Insert(singleKey));
            return newHeap;
        }

        public void TestAllForHeapOfN(int numOfElements)
        {
            List<FibonacciNode> allNodes;
            FibonacciHeap heap = GenerateRandomHeap(numOfElements, out allNodes);
            Console.WriteLine($"Test for Fibonacci heap of {numOfElements} elements\n");
            TestDecreaseKeyForNElements(heap, allNodes, numOfElements / 10);
            TestDeleteNodeForNElements(heap, allNodes, numOfElements / 10);
            allNodes.AddRange(TestInsertionForNElements(heap, numOfElements / 10));
            TestExtractForNElements(heap, allNodes.Count / 10);
            Console.WriteLine();
        }
    }
}
