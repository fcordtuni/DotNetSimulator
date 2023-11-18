//Author: FCORDT
namespace DotNetSimulator.Utils
{
    /// <summary>
    /// Bare-bone implementation of a DAG:
    ///  * add nodes
    ///  * add edges
    ///  * cycle detection 
    ///  * topological sorting
    /// NOT Implemented (and not needed by simulator):
    ///  * Removing of Nodes
    ///  * Removing of Edges
    ///  * Weighted Edges
    ///  * Any sort of advanced Graph Algorithms
    ///  
    /// </summary>
    /// <typeparam name="T">The Type of the Node.</typeparam>
    internal class DAG<T> where T : class
    {

        private readonly ISet<T> _nodes = new HashSet<T>();
        private readonly IDictionary<T, HashSet<T>> _incomingEdges = new Dictionary<T, HashSet<T>>();
        private readonly IDictionary<T, HashSet<T>> _outgoingEdges = new Dictionary<T, HashSet<T>>();

        public void AddNode(T node)
        {
            _nodes.Add(node);
        }

        public void AddEdge(T? fromNode, T? toNode)
        {
            if (fromNode == null) throw new ArgumentNullException(nameof(fromNode));
            if (toNode == null) throw new ArgumentNullException(nameof(toNode));
            AddNode(fromNode);
            AddNode(toNode);

            //is there a way to add it more easily, like javas compute if absent?
            if (!_outgoingEdges.ContainsKey(fromNode)) _outgoingEdges.Add(fromNode, new HashSet<T>());
            _outgoingEdges[fromNode].Add(toNode);
            if(!_incomingEdges.ContainsKey(toNode)) _incomingEdges.Add(toNode, new HashSet<T>());
            _incomingEdges[toNode].Add(fromNode);
        }

        public ISet<T> OutgoingNodes(T node)
        {
            return _outgoingEdges.TryGetValue(node, out var outgoingNodes) ? outgoingNodes : new HashSet<T>();
        }

        public ISet<T> IncomingNodes(T node)
        {
            return _incomingEdges.TryGetValue(node, out var incomingNodes) ? incomingNodes : new HashSet<T>();
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
            foreach(var node in _nodes)
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
                var nextNode = freeNodes.Dequeue();
                foreach (var connectingNode in OutgoingNodes(nextNode))
                {
                    incomingEdgeCount[connectingNode]--;
                    if (incomingEdgeCount[connectingNode] != 0) continue;
                    freeNodes.Enqueue(connectingNode);
                    incomingEdgeCount.Remove(connectingNode);
                }
                sortedNodes.Add(nextNode);
            }

            if(incomingEdgeCount.Count > 0) { throw new InvalidOperationException("Graph contains cycles!"); }

            return sortedNodes;
        }
    }
}
