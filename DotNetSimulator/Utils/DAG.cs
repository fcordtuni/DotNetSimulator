using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Utils
{
    /// <summary>
    /// Barebone implementation of a DAG:
    ///  * add nodes
    ///  * add edges
    ///  * cycle detection 
    ///  * topological sorting
    /// NOT Implemented (and not needed by simulator):
    ///  * Removing of Nodes
    ///  * Removind of Edges
    ///  * Weighted Edges
    ///  * Any sort of advanced Graph Algorithms
    ///  
    /// </summary>
    /// <typeparam name="T">The Type of the Node.</typeparam>
    internal class DAG<T> where T : class
    {

        private readonly ISet<T> nodes;
        private readonly IDictionary<T, HashSet<T>> incomingEdges;
        private readonly IDictionary<T, HashSet<T>> outgoingEdges;

        public DAG() 
        {
            nodes = new HashSet<T>();
            incomingEdges = new Dictionary<T, HashSet<T>>();
            outgoingEdges = new Dictionary<T, HashSet<T>>();
        }

        public void AddNode(T node)
        {
            if (nodes.Contains(node)) throw new ArgumentException("Node already exists!");
            nodes.Add(node);
        }

        public void AddEdge(T? fromNode, T? toNode)
        {
            if (fromNode == null) throw new ArgumentNullException(nameof(fromNode));
            if (toNode == null) throw new ArgumentNullException(nameof(toNode));
            if (!nodes.Contains(fromNode)) AddNode(fromNode);
            if (!nodes.Contains(toNode)) AddNode(toNode);

            //is there a way to add it more easily, like javas compute if absent?
            if(!outgoingEdges.ContainsKey(fromNode)) outgoingEdges.Add(fromNode, new HashSet<T>());
            outgoingEdges[fromNode].Add(toNode);
            if(!incomingEdges.ContainsKey(toNode)) incomingEdges.Add(toNode, new HashSet<T>());
            incomingEdges[toNode].Add(fromNode);
        }

        public ISet<T> OutgoingNodes(T node)
        {
            if(outgoingEdges.ContainsKey(node))
            {
                return outgoingEdges[node];
            }
            return new HashSet<T>();
        }

        public ISet<T> IncomingNodes(T node) 
        { 
            if(incomingEdges.ContainsKey(node))
            {
                return incomingEdges[node]; 
            }
            return new HashSet<T>();
        }

        /// <summary>
        /// Creating a Topological Sort of the DAG using Kahn's algorithm
        /// </summary>
        /// <returns>the topological sorted graph as list of nodes</returns>
        public List<T> TopologicalSort()
        {
            List<T> sortedNodes = new();
            Dictionary<T, int> incomingEdgeCount = new();
            Queue<T> freeNodes = new();

            //setup: create data structures
            foreach(T node in nodes)
            {
                var incomingNodes = IncomingNodes(node);
                if(incomingNodes.Count == 0)
                {
                    freeNodes.Enqueue(node);
                }
                else
                {
                    incomingEdgeCount[node] = incomingNodes.Count;
                }

            }

            //running algorithm
            while (freeNodes.Count > 0)
            {
                T nextNode = freeNodes.Dequeue();
                foreach (T connectingNode in OutgoingNodes(nextNode))
                {
                    incomingEdgeCount[connectingNode]--;
                    if (incomingEdgeCount[connectingNode] == 0)
                    {
                        freeNodes.Enqueue(connectingNode);
                        incomingEdgeCount.Remove(connectingNode);
                    }
                }
                sortedNodes.Add(nextNode);
            }

            if(incomingEdgeCount.Count > 0) { throw new InvalidOperationException("Graph contains cycles!"); }

            return sortedNodes;
        }
    }
}
