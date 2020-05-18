using System.Collections.Generic;

namespace AlgoLib.BTrees
{
    public class BpTreeNode
    {
        private readonly int _maxOrder;
        private BpTreeNode parent;
        private BpTreeNode leftSibiling;
        private BpTreeNode righSibiling;
        public List<int> Keys { get; private set; }
        public List<BpTreeNode> Children { get; private set; }
        public int KeysCount { get; private set; }
        public BpTreeNode Parent { get => parent; }
        public BpTreeNode LeftSibiling { get => leftSibiling; }
        public BpTreeNode RightSibiling { get => righSibiling; }
        public int MedianIndex { get => Keys.Count / 2; }

        public BpTreeNode(int maxOrder, BpTreeNode parent)
        {
            this.parent = parent;
            _maxOrder = maxOrder;
            leftSibiling = righSibiling = null;
            KeysCount = 0;
            Keys = new List<int>(maxOrder);
            for (int i = 0; i < Keys.Capacity; i++)
                Keys.Add(int.MaxValue);
            Children = new List<BpTreeNode>(maxOrder + 1);
            for(int i = 0; i < Children.Capacity; i++)
                Children.Add(default);
        }

        public void InsertValue(int newValue)
        {
            int indexOfInsertion = FindIndexToInsertAt(newValue);
            InsertSingleNumber(Keys, indexOfInsertion, newValue);
            KeysCount++;
            if (KeysCount != _maxOrder)
                return;
            BpTreeNode leftNode, rightNode;
            int value = Split(out leftNode,out rightNode);
            int parentIndexToInsert = parent.FindIndexToInsertAt(value);
            parent.Children.RemoveAt(parentIndexToInsert);
            InsertChildren(parent.Children, parentIndexToInsert, rightNode, leftNode);
            parent.InsertValue(value);
        }

        public int Split(out BpTreeNode newLeft, out BpTreeNode newRight)
        {
            BpTreeNode newLeftNode = new BpTreeNode(_maxOrder, this);
            BpTreeNode newRightNode = new BpTreeNode(_maxOrder, this);
            newLeftNode.InsertValuesInNode(Keys, 0, MedianIndex);
            if (IsLeaf())
                newRightNode.InsertValuesInNode(Keys, MedianIndex, Keys.Count - MedianIndex);
            else
                newRightNode.InsertValuesInNode(Keys, MedianIndex + 1, Keys.Count - MedianIndex - 1);
            newLeftNode.righSibiling = newRightNode;
            newRightNode.leftSibiling = newLeftNode;
            if (parent == null)
                parent = new BpTreeNode(_maxOrder, null);
            newLeftNode.parent = parent;
            newRightNode.parent = parent;
            LinkNewSibilings(this, newLeftNode, newRightNode);
            if (!IsLeaf())
            {
                for (int i = 0; i <= MedianIndex; i++)
                    newLeftNode.Children[i] = Children[i];
                for (int i = MedianIndex + 1; i < _maxOrder + 1; i++)
                    newRightNode.Children[i - MedianIndex -1] = Children[i];
            }
            int valueToReturn = Keys[MedianIndex];
            newLeft = newLeftNode;
            newRight = newRightNode;
            return valueToReturn;
        }

        public bool IsLeaf()
        {
            int index = 0;
            int numOfDefaults = 0;
            while (index < Children.Count)
            {
                if (Children[index] == default(BpTreeNode))
                    numOfDefaults++;
                index++;
            }
            return numOfDefaults == Children.Count;
        }

        private void LinkNewSibilings(BpTreeNode currentNode, BpTreeNode newLeftSibiling, BpTreeNode newRightSibiling)
        {
            if (currentNode.righSibiling != null)
            {
                newRightSibiling.righSibiling = currentNode.righSibiling;
                currentNode.righSibiling.leftSibiling = newRightSibiling;
            }
            if(currentNode.leftSibiling != null)
            {
                newLeftSibiling.leftSibiling = currentNode.leftSibiling;
                currentNode.leftSibiling.righSibiling = newLeftSibiling;
            }
        }

        private int FindIndexToInsertAt(int valueToInsert)
        {
            int index = 0;
            while (index < KeysCount && valueToInsert > Keys[index])
                index++;
            return index;
        }

        private void InsertValuesInNode(List<int> valuesToInsert, int startingIndex, int numOfInsertions)
        {
            for (int i = 0; i < numOfInsertions; i++)
                InsertValue(valuesToInsert[startingIndex + i]);
        }

        private void InsertSingleNumber(List<int> listToInsertTo,int index, int value)
        {
            for (int i = listToInsertTo.Count -1; i > index; i--)
                listToInsertTo[i] = listToInsertTo[i-1];
            listToInsertTo.RemoveAt(index);
            listToInsertTo.Insert(index, value);
        }

        private void InsertChildren(List<BpTreeNode> listToInsertTo, int index, params BpTreeNode[] children)
        {
            foreach (BpTreeNode newChild in children)
                listToInsertTo.Insert(index, newChild);
            for (int i = 0; i < children.Length - 1; i++)
                listToInsertTo.RemoveAt(listToInsertTo.Count - 1);
        }
    }
}
