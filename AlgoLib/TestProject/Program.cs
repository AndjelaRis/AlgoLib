using AlgoLib.BTrees;
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
            BpTree tree = new BpTree();
            tree.Insert(30);
            tree.Insert(50);
            tree.Insert(80);
            tree.Insert(100);
            tree.Insert(45);
            tree.Insert(35);
            bool result = tree.Contains(50);
            List<int> interval = tree.FindInterval(80, 120);
        }
    }
}
