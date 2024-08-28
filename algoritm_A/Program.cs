using System;
using System.Collections.Generic;
public class Node
{
    public int x;
    public int y;
    public bool isObstacle;
    public Node parent;
    public double gCost;
    public double hCost;
    public double fCost { get { return gCost + hCost; } }
    public Node(int x, int y, bool isObstacle)
    {
        this.x = x;
        this.y = y;
        this.isObstacle = isObstacle;
    }
}
public class AStar
{
    private static double CalculateDistance(Node a, Node b)
    {
        int dx = Math.Abs(a.x - b.x);
        int dy = Math.Abs(a.y - b.y);
        return Math.Sqrt(dx * dx + dy * dy);
    }
    public static List<Node> FindPath(Node startNode, Node targetNode)
    {
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
        
            if (currentNode == targetNode)
            {
                return GeneratePath(startNode, targetNode);
            }
            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (neighbor.isObstacle || closedSet.Contains(neighbor))
                {
                    continue;
                }
                double newMovementCostToNeighbor = currentNode.gCost + CalculateDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = CalculateDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }
    private static List<Node> GeneratePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }
private static List<Node> GetNeighbors(Node node)
{
    List<Node> neighbors = new List<Node>();
    int startX = Math.Max(0, node.x - 1);
    int endX = Math.Min(node.x + 1, Map.Width - 1);

    int startY = Math.Max(0, node.y - 1);
        int endY = Math.Min(node.y + 1, Map.Height - 1);
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (x == node.x && y == node.y)
                {
                    continue;
                }
                neighbors.Add(Map.nodes[x, y]);
            }
        }
        return neighbors;
    }
}
public class Map
{
    public static int Width { get; private set; }
    public static int Height { get; private set; }
    public static Node[,] nodes;
    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        nodes = new Node[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                bool isObstacle = false; // Set to true if there's an obstacle at this position
                nodes[x, y] = new Node(x, y, isObstacle);
            }
        }
    }
}
public class Program
{
    public static void Main()
    {
        Map map = new Map(10, 10);
        Node startNode = Map.nodes[0, 0];
        Node targetNode = Map.nodes[9, 5];
        List<Node> path = AStar.FindPath(startNode, targetNode);
        foreach (Node node in path)
        {
            Console.WriteLine($"({node.x}, {node.y})");
        }
    }
}