using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApplication1
{
    [SuppressMessage("ReSharper", "TooWideLocalVariableScope")]
    internal class Program
    {
        public static void Main(string[] args)
        {
            #region Buffer

            int counter1, counter2;

            #endregion

            #region Fill apexes

            
            //var input = Console.ReadLine();
            var input = "2 1 6 2 6";
            
            
            // ReSharper disable once PossibleNullReferenceException
            var apexSet = input.Split(' ').Select(x => int.Parse(x)).ToArray();
            var apexesCount = apexSet.Count();
            var apexes = new int[apexesCount, 2];
            
            for (counter1 = 0; counter1 < apexesCount; counter1++)
            {
                apexes[counter1, 0] = apexSet[counter1];
            }
            
            #endregion
            
            
            // [x, 0] - value 
            // [x, 1] - isFreeNode
            // [x, 2] - relation

            var nodesCount = 0; // max value in apexes array
            
            for (counter1 = 0; counter1 < apexes.Length; counter1++)
            {
                if (apexes[counter1, 0] > nodesCount)
                    nodesCount = apexes[counter1, 0];
            }

            //var nodes = new int[nodesCount, 3];

            bool isFound;
            var freeNodes = new List<int[]>(3000);
            
            for (counter1 = 1; counter1 <= nodesCount; counter1++)
            {
                isFound = false;
                
                for (counter2 = 0; counter2 < apexes.Length; counter2++)
                {
                    if (apexes[counter2, 0] == counter1)
                    {
                        isFound = true;
                        break;
                    }
                }
                
                if(!isFound)
                    freeNodes.Add(new [] { counter1, 0 });
            }
            
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            
            int minimalNode;

            bool isFreeNodeUsed = false;
            int elementIndexThatWasUsed = -1;
            
            for (counter1 = 0; counter1 < apexesCount; counter1++)
            {
                minimalNode = nodesCount + 1;
                
                // Finds minimal free node
                for (counter2 = 0; counter2 < freeNodes.Count; counter2++)
                {
                    if (freeNodes[counter2][1] == 0 && minimalNode > freeNodes[counter2][0])
                    {
                        minimalNode = freeNodes[counter2][0];
                        isFreeNodeUsed = true;
                        elementIndexThatWasUsed = counter2;
                    }
                }
                
                // Compares with lowest apex
                bool isUsed;
                
                for (counter2 = 0; counter2 < counter1; counter2++)
                {
                    isUsed = false;
                    
                    if (apexes[counter2, 1] == 0 && apexes[counter2, 0] < minimalNode)
                    {
                        for (int counter3 = counter2 + 1; counter3 < apexesCount; counter3++)
                        {
                            if (apexes[counter3, 0] == apexes[counter2, 0])
                            {
                                isUsed = true;
                                break;
                            }
                        }
                        
                        if(!isUsed)
                        {
                            minimalNode = apexes[counter2, 0];
                            isFreeNodeUsed = false;
                            elementIndexThatWasUsed = counter2;
                        }
                    }
                }
                
                if (isFreeNodeUsed)
                {
                    apexes[elementIndexThatWasUsed, 1] = minimalNode;
                }
                else
                {
                    freeNodes[elementIndexThatWasUsed][1] = minimalNode;
                }
            }

            
            
            for (counter1 = 0; counter1 < nodesCount; counter1++)
            {
                var relationList = new List<int>(100);
                
                for (counter2 = 0; counter2 < freeNodes.Count; counter2++)
                {
                    if (freeNodes[counter2][0] == counter1)
                    {
                        relationList.Add(freeNodes[counter2][1]);
                    } else if (freeNodes[counter2][1] == counter1)
                    {
                        relationList.Add(freeNodes[counter2][0]);
                    }
                }

                for (counter2 = 0; counter2 < apexesCount; counter2++)
                {
                    if (apexes[counter2, 0] == counter1)
                    {
                        relationList.Add(apexes[counter2, 1]);
                    } else if (apexes[counter2, 1] == counter1)
                    {
                        relationList.Add(apexes[counter2, 0]);
                    }
                }
                
                var strBuilder = new StringBuilder();

                strBuilder.Append(counter1);
                strBuilder.Append(": ");
                
                for (counter2 = 0; counter2 < relationList.Count; counter2++)
                {
                    strBuilder.Append(relationList[counter2]);
                    strBuilder.Append(" ");
                }
                
                Console.WriteLine(strBuilder.ToString().Trim());
            }
        }
    }
}