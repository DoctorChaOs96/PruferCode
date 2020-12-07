using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        private class Node
        {
            private Node _parent;
            private List<Node> _children;

            public Node(int value)
            {
                _parent = null;
                _children = new List<Node>();
                Value = value;
            }

            public int Value { get; }

            public bool IsTaken()
            {
                return _parent != null;
            }

            public bool IsRelated(int relation)
            {
                return _parent?.Value == relation || _children.Any(ch => ch.Value == relation);
            }

            public void AssignParent(Node relation)
            {
                _parent = relation;
            }

            public void AssignChild(Node relation)
            {
                _children.Add(relation);
            }

            public IEnumerable<int> GetAllRelations()
            {
                var outList = new List<Node>(1000);
                
                if(_parent != null)
                    outList.Add(_parent);
                
                _children.ForEach(ch => outList.Add(ch));

                return outList.OrderBy(n => n.Value).Select(n => n.Value);
            }

            public override string ToString()
            {
                return this.Value.ToString();
            }
        }

        static void Main(string[] args)
        {
            //var baseApexesValues = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var baseApexesValues = "2 1 6 2 6".Split(' ').Select(int.Parse).ToArray();
            
            var nodes = Enumerable.Range(1, baseApexesValues.Length + 1).Select(x => new Node(x)).ToArray();
            
            var baseNodes = baseApexesValues.Select(bnv => nodes.First(n => n.Value == bnv)).ToArray();
            var boundaryNodes = nodes.Where(n => !baseNodes.Any(bn => bn.Value == n.Value)).ToArray();

            for (int i = 0; i < baseNodes.Length; i++)
            {
                var baseNode = baseNodes[i];

                var possibleNodes = new List<Node>(boundaryNodes.Where(n => !n.IsTaken()));
                
                for (int j = 0; j < i; j++)
                {
                    var freeBaseNode = baseNodes[j];

                    if (freeBaseNode.IsTaken())
                    {
                        continue;
                    }
                    
                    bool willBeUsed = false;
                    
                    for (int k = i; k < baseNodes.Length; k++)
                    {
                        if (baseNodes[k].Value == freeBaseNode.Value)
                        {
                            willBeUsed = true;
                            break;
                        }
                    }
                    
                    if(!willBeUsed)
                        possibleNodes.Add(freeBaseNode);
                }


                Node lowestNode = new Node(75002);
                
                foreach (var node in possibleNodes)
                {
                    if (lowestNode.Value > node.Value)
                    {
                        lowestNode = node;
                    }
                }
                
                lowestNode.AssignParent(baseNode);
                baseNode.AssignChild(lowestNode);
            }

            foreach (var node in nodes)
            {
                Console.WriteLine($"{node.Value}: {string.Join(" ", nodes.Where(n => n.IsRelated(node.Value)))}");
                //Console.WriteLine($"{node.Value}: {string.Join(" ", node.GetAllRelations())}");
            }

            //Console.ReadKey();
        }
    }
}
