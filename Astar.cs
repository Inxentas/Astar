using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * For this class I've followed this Javascript guide and rewrote the code in C#:
 * guide: https://briangrinstead.com/blog/astar-search-algorithm-in-javascript/
 */

[Serializable]
public class Astar
{
    private Map _map;
    public Map map { get { return _map; } }

    public List<MapNode> openList = new List<MapNode>();
    public List<MapNode> closedList = new List<MapNode>();
    public MapNode startNode;
    public MapNode endNode;
    public int maxIterations = 1000;

    public void SetMap(Map map)
    {
        this._map = map;
        foreach (MapNode node in _map.nodes)
        {
            closedList.Add(node);
        }
    }
    public void SetStart(MapNode node)
    {
        startNode = node;
    }
    public void SetEnd(MapNode node)
    {
        endNode = node;
    }
    public float Manhattan(MapNode nodeA, MapNode nodeB)
    {
        var x = Mathf.Abs(nodeB.position.x - nodeA.position.x);
        var z = Mathf.Abs(nodeB.position.z - nodeA.position.z);
        return x + z;
    }
    public MapNode GetLowestFInOpenList()
    {
        MapNode lowest = openList[0];
        MapNode target = lowest;
        foreach (MapNode node in openList)
        {
            if (node.F < lowest.F)
            {
                target = lowest;
            }
        }
        return lowest;
    }
    public List<MapNode> GetPath()
    {
        if (startNode == null)
        {
            Debug.Log(this + ": startNode was null.");
            return null;
        }

        if (endNode == null)
        {
            Debug.Log(this + ": endNode was null.");
            return null;
        }

        if (!startNode.walkable)
        {
            Debug.Log(this + ": startNode was not walkable.");
            return null;
        }

        if (!endNode.walkable)
        {
            Debug.Log(this + ": endNode was not walkable.");
            return null;
        }

        int safe = maxIterations;

        // Debug.Log("GetPath from: " + startNode.name + " to " + endNode.name);

        // start the open and closed Lists and
        // add the fist node to the openList.
        openList = new List<MapNode>();
        closedList = new List<MapNode>();
        openList.Add(startNode);

        // iterate through the openList until it is empty.
        while (openList.Count > 0 && safe > 0)
        {
            MapNode q = GetLowestFInOpenList();
            // end case: result has been found, return the (reversed) path.
            if (q == endNode)
            {
                MapNode pathNode = q;
                List<MapNode> pathNodes = new List<MapNode>();
                while (pathNode.parentNode != null)
                {
                    pathNodes.Add(pathNode);
                    pathNode = pathNode.parentNode;
                }
                pathNodes.Reverse();
                return pathNodes;
            }

            // default case: move currentNode from open to closed, process each of its neighbors.
            else
            {
                openList.Remove(q);
                closedList.Add(q);
                MapNode[] neighbors = q.GetNeighbors();
                foreach (MapNode neighbor in neighbors)
                {
                    if (closedList.Contains(neighbor) || !neighbor.walkable)
                    {
                        // not a valid node to process, skip to next neighbor
                        // Debug.Log("- " + neighbor.name + ": will be skipped, it's on the closed list/unwalkable.");
                        continue; // IMPORTANT
                    }

                    // Debug.Log("- " + neighbor.name + ": will be processed as neighbor.");

                    // g score is the shortest distance from start to current node, we need to check if
                    // the path we have arrived at this neighbor is the shortest one we have seen yet
                    var gScore = q.G + 1; // 1 is the distance from a node to it's neighbor
                    bool bestG = false;

                    if (!openList.Contains(neighbor))
                    {
                        // This the the first time we have arrived at this node, it must be the best
                        // Also, we need to take the h (heuristic) score since we haven't done so yet
                        bestG = true;
                        neighbor.H = Manhattan(neighbor, endNode);
                        openList.Add(neighbor);
                    }
                    else if (gScore < neighbor.G)
                    {
                        // We have already seen the node, but last time it had a worse g (distance from start)
                        bestG = true;
                    }

                    if (bestG)
                    {
                        // Found an optimal (so far) path to this node. We update it's scores and parent so
                        // we can trace the final path back later if it contains the node from this iteration.
                        neighbor.parentNode = q;
                        neighbor.G = gScore;
                        neighbor.F = neighbor.G + neighbor.H;
                    }
                }
            }
            safe--;
        }
        return null;
    }
}