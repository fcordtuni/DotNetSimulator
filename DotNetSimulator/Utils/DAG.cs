//Author: FCORDT
namespace DotNetSimulator.Utils;
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

    /// <summary>
    /// adds a node to the graph
    /// </summary>
    /// <param name="node"></param>
    private void AddNode(T node)
    {
        _nodes.Add(node);
    }

    /// <summary>
    /// adds an edge between two given nodes
    /// </summary>
    /// <param name="fromNode"></param>
    /// <param name="toNode"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void AddEdge(T fromNode, T toNode)
    {
        AddNode(fromNode);
        AddNode(toNode);

        //is there a way to add it more easily, like javas compute if absent?
        if (!_outgoingEdges.ContainsKey(fromNode)) _outgoingEdges.Add(fromNode, []);
        _outgoingEdges[fromNode].Add(toNode);
        if (!_incomingEdges.ContainsKey(toNode)) _incomingEdges.Add(toNode, []);
        _incomingEdges[toNode].Add(fromNode);
    }

    /// <summary>
    /// get a list of nodes the current node has outgoing edges to
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private IEnumerable<T> OutgoingNodes(T node)
    {
        return _outgoingEdges.TryGetValue(node, out var outgoingNodes) ? outgoingNodes : [];
    }

    /// <summary>
    /// gets a list of nodes which this node has incoming edges from
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public ISet<T> IncomingNodes(T node)
    {
        return _incomingEdges.TryGetValue(node, out var incomingNodes) ? incomingNodes : [];
    }

    /// <summary>
    /// Creating a Topological Sort of the DAG using Kahn's algorithm
    /// </summary>
    /// <returns>the topological sorted graph as list of nodes</returns>
    public List<T> TopologicalSort()
    {
        List<T> sortedNodes = [];
        Dictionary<T, int> incomingEdgeCount = [];
        Queue<T> freeNodes = new();

        //setup: create data structures
        foreach (var node in _nodes)
        {
            var incomingNodes = IncomingNodes(node);
            if (incomingNodes.Count == 0)
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

        if (incomingEdgeCount.Count > 0) { throw new InvalidOperationException("Graph contains cycles!"); }

        return sortedNodes;
    }
}
