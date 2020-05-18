using System;
using System.Collections.Generic;
using System.Data;

namespace AlgoLib.BTrees
{
    public class BpTree
    {
        private const int maxOrder = 3;
        BpTreeNode Root { get; set; }
    
        public BpTree()
        {
            
        }

        public void Insert(int newValue)
        {
            BpTreeNode nodeToInsertTo = null;
            if (Root == null)
            {
                Root = new BpTreeNode(maxOrder, null);
                nodeToInsertTo = Root;
            }
            else
                nodeToInsertTo = FindNodeToInsertIn(Root, newValue);
            nodeToInsertTo.InsertValue(newValue);
            Root = FindNewRoot();
        }

        public BpTreeNode FindNodeToInsertIn(BpTreeNode rootOfTree, int newValue)
        {
            if (rootOfTree.IsLeaf())
                return rootOfTree;
            for (int i = 0; i < rootOfTree.KeysCount; i++)
            {
                if (newValue < rootOfTree.Keys[i])
                    return FindNodeToInsertIn(rootOfTree.Children[i], newValue);
                if (rootOfTree.KeysCount == i + 1)
                    return FindNodeToInsertIn(rootOfTree.Children[i + 1], newValue);
            }
            return rootOfTree;
        }

        public BpTreeNode FindNewRoot()
        {
            BpTreeNode currentNode = Root;
            while (currentNode.Parent != null)
                currentNode = currentNode.Parent;
            return currentNode;
        }

        public bool Contains(int value)=>FindNode(value) != default(BpTreeNode);

        public BpTreeNode FindNode(int value)=> FindNode(Root, value);

        private BpTreeNode FindNode(BpTreeNode subTreeRoot, int value)
        {
            int index = 0;
            while (index < subTreeRoot.KeysCount && value > subTreeRoot.Keys[index])
                index++;
            if (subTreeRoot.Keys[index] == value)
                if(subTreeRoot.IsLeaf())
                    return subTreeRoot;
                else
                    return FindNode(subTreeRoot.Children[index+1], value);
            if (!subTreeRoot.IsLeaf())
                return FindNode(subTreeRoot.Children[index], value);
            else
                return default(BpTreeNode);
        }

        public List<int> FindInterval(int lowerLimit, int upperLimit)
        {
            if (upperLimit <= lowerLimit)
                return null;
            BpTreeNode nodeToTarget = FindNodeToInsertIn(Root, lowerLimit);
            int index = 0;
            while (index < nodeToTarget.KeysCount && nodeToTarget.Keys[index] < lowerLimit)
                index++;
            List<int> result = new List<int>();
            while (nodeToTarget.Keys[index] < upperLimit)
            {
                result.Add(nodeToTarget.Keys[index]);
                index++;
            }
            while(nodeToTarget.RightSibiling != default(BpTreeNode))
            {
                nodeToTarget = nodeToTarget.RightSibiling;
                index = 0;
                while (nodeToTarget.Keys[index] < upperLimit)
                {
                    result.Add(nodeToTarget.Keys[index]);
                    index++;
                }
            }
            return result;
        }
    }
}
