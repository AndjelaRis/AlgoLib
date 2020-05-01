using System;
using System.Collections.Generic;

namespace AlgoLib.Heaps.Fibonacci
{
    public class FibonacciHeap
    {
        private int numOfNodes;
        public FibonacciNode RootList { get; private set; }
        public FibonacciNode MinNode { get; private set; }

        public List<FibonacciNode> Iterate(FibonacciNode head)
        {
            if (head == null)
                return null;
            List<FibonacciNode> iteratedNodes = new List<FibonacciNode>();
            FibonacciNode currentNode, endNode;
            currentNode = endNode = head;
            do
            {
                iteratedNodes.Add(currentNode);
                currentNode = currentNode.Right;
            } while (currentNode != endNode);
            return iteratedNodes;
        }
        
        public FibonacciNode ExtractMin()
        {
            FibonacciNode currentMin = MinNode;
            if (currentMin == null)
                return null;
            if(currentMin.Child != null)
            {
                List<FibonacciNode> childrenOfMin = Iterate(currentMin.Child);
                foreach(FibonacciNode child in childrenOfMin)
                {
                    MergeWithRootList(child);
                    child.Parent = null;
                }
            }
            RemoveFromRootList(currentMin);
            if (currentMin == currentMin.Right)
                MinNode = RootList = null;
            else
            {
                MinNode = currentMin.Right;
                Consolidate();
            }
            numOfNodes--;
            return currentMin;
        }

        public void Insert(int newKey)
        {
            FibonacciNode newNode = new FibonacciNode(newKey);
            newNode.Left = newNode.Right = newNode;
            MergeWithRootList(newNode);
            if (MinNode == null || newNode.Key < MinNode.Key)
                MinNode = newNode;
            numOfNodes++;
        }

        public void DecreaseKey(FibonacciNode nodeToChange, int key)
        {
            if (key > nodeToChange.Key)
                return;
            nodeToChange.Key = key;
            if(nodeToChange.Parent != null && nodeToChange.Key < nodeToChange.Parent.Key)
            {
                Cut(nodeToChange, nodeToChange.Parent);
                CascadeCut(nodeToChange.Parent);
            }
            if (nodeToChange.Key < MinNode.Key)
                MinNode = nodeToChange;
        }

        public FibonacciHeap Merge(FibonacciHeap heapToMerge)
        {
            FibonacciHeap newHeap = new FibonacciHeap();
            newHeap.RootList = RootList;
            newHeap.MinNode = MinNode;
            FibonacciNode lastNode = heapToMerge.RootList.Left;
            heapToMerge.RootList.Left = newHeap.RootList.Left;
            newHeap.RootList.Right = heapToMerge.RootList;
            newHeap.RootList.Left = lastNode;
            newHeap.RootList.Left.Right = newHeap.RootList;
            if (heapToMerge.MinNode.Key < newHeap.MinNode.Key)
                newHeap.MinNode = heapToMerge.MinNode;
            newHeap.numOfNodes = numOfNodes + heapToMerge.numOfNodes;
            return newHeap;
        }

        private void Consolidate()
        {
            List<FibonacciNode> helperList = new List<FibonacciNode>(numOfNodes);
            for (int i = 0; i < numOfNodes;i++)
                helperList.Add(null);
            List<FibonacciNode> rootNodes = Iterate(RootList);
            for(int i = 0; i < rootNodes.Count; i++)
            {
                int d = rootNodes[i].Degree;
                while (helperList[d] != null)
                {
                    if (rootNodes[i].Key > helperList[d].Key)
                    {
                        FibonacciNode temp = rootNodes[i];
                        rootNodes[i] = helperList[d];
                        helperList[d] = temp;
                    }
                    HeapLink(helperList[d], rootNodes[i]);
                    helperList[d] = null;
                    d++;
                }
                helperList[d] = rootNodes[i];
            }
            foreach (FibonacciNode singleNode in helperList)
                if (singleNode != null && singleNode.Key < MinNode.Key)
                    MinNode = singleNode;
        }

        private void HeapLink(FibonacciNode firstNode, FibonacciNode secondNode)
        {
            RemoveFromRootList(firstNode);
            firstNode.Left = firstNode.Right = firstNode;
            MergeWithChildList(secondNode, firstNode);
            secondNode.Degree++;
            firstNode.Parent = secondNode;
            firstNode.IsMarked = false;
        }

        private void MergeWithChildList(FibonacciNode parentNode, FibonacciNode node)
        {
            if (parentNode.Child != null)
            {
                node.Right = parentNode.Child.Right;
                node.Left = parentNode.Child;
                parentNode.Child.Right.Left = node;
                parentNode.Child.Right = node;
            }
            else
                parentNode.Child = node;
        }

        private void Cut(FibonacciNode childNode, FibonacciNode parentNode)
        {
            RemoveFromChildList(parentNode, childNode);
            parentNode.Degree--;
            MergeWithRootList(childNode);
            childNode.Parent = null;
            childNode.IsMarked = false;
        }

        private void CascadeCut(FibonacciNode currentNode)
        {
            FibonacciNode parentNode = currentNode.Parent;
            if (parentNode == null)
                return;
            if (parentNode.IsMarked)
            {
                Cut(parentNode, currentNode);
                CascadeCut(currentNode);
            }
            else
                parentNode.IsMarked = true;
        }

        private void RemoveFromChildList(FibonacciNode parentNode, FibonacciNode childNode)
        {
            if (parentNode.Child == parentNode.Child.Right)
                parentNode.Child = null;
            else if(parentNode.Child == childNode)
            {
                parentNode.Child = childNode.Right;
                childNode.Right.Parent = parentNode;
            }
            childNode.Left.Right = childNode.Right;
            childNode.Right.Left = childNode.Left;
        }

        private void RemoveFromRootList(FibonacciNode node)
        {
            if (node == RootList)
                RootList = node.Right;
            node.Left.Right = node.Right;
            node.Right.Left = node.Left;
        }

        private void MergeWithRootList(FibonacciNode node)
        {
            if (RootList == null)
            {
                RootList = node;
                return;
            }
            node.Right = RootList.Right;
            node.Left = RootList;
            RootList.Right.Left = node;
            RootList.Right = node;
        }
    }
}
