using AlgoLib.Heaps.Fibonacci;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    class Program
    {
       static void Main(string[] args)
        {
            FibonacciReportEngine engine = new FibonacciReportEngine(0, 10000);

            engine.TestAllForHeapOfN(10);
            engine.TestAllForHeapOfN(100);
            engine.TestAllForHeapOfN(1000);
            engine.TestAllForHeapOfN(10000);
            engine.TestAllForHeapOfN(100000);
        }
    }
}
