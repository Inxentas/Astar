using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Map
{
    [Tooltip("The amount of nodes in the X direction.")]
    public int width;
    [Tooltip("The amount of nodes in the Z direction.")]
    public int depth;
    [Tooltip("The scale of the map and it's nodes.")]
    public float scale;

    public MapNode[] _nodes;
    public MapNode[] nodes { get { return _nodes; } }

    public Map(int width, int depth, float scale = 1)
    {
        GenerateMapData(width, depth, scale);
    }

    #region Generating Map Data

    /*
     * Generates the map data by generating MapNodes. The width and depth
     * determine how many nodes will be generated, and with the scale option
     * you can control the distance between each node (the default is 1).
     */
    public void GenerateMapData(int width, int depth, float scale = 1)
    {
        this.width = width;
        this.depth = depth;
        this.scale = scale;

        this._nodes = new MapNode[width * depth];
        int counter = 0;
        for (int w = 0; w < width; w++)
        {
            for (int d = 0; d < depth; d++)
            {
                MapNode node = new MapNode(w, d, scale);
                _nodes[counter] = node;
                counter++;
            }
        }
        ConnectMapData();
    }

    /**
     * Once our map has been generated, we iterate through it once to set
     * each of the nodes neighbors. This costs some performance, but saves
     * us from running a ton of lookup code every time a path is requested.
     */
    private void ConnectMapData()
    {
        for (int i = 0; i < this.nodes.Length; i++)
        {
            MapNode node = GetNodeAtIndex(i);
            node.left = GetNodeAtPosition(node.posX - 1, node.posZ);
            node.right = GetNodeAtPosition(node.posX + 1, node.posZ);
            node.back = GetNodeAtPosition(node.posX, node.posZ - 1);
            node.forward = GetNodeAtPosition(node.posX, node.posZ + 1);
        }
    }

    #endregion

    #region Filtering Map Nodes

    /**
     * Returns the node at the given index. Returns null if out of bounds.
     */
    public MapNode GetNodeAtIndex(int index)
    {
        if (index < this.nodes.Length)
        {
            return this.nodes[index];
        }
        return null;
    }

    /**
     * Returns the node at the given coordinate. Returns null if out of bounds.
     */
    public MapNode GetNodeAtPosition(int posX, int posZ)
    {
        foreach (MapNode node in nodes)
        {
            if (node.posX == posX && node.posZ == posZ)
            {
                return node;
            }
        }
        return null;
    }

    /*
     * Returns all nodes on the edge of the map according to a thickness value.
     */
    public List<MapNode> GetEdgeNodes(int thickness = 1)
    {
        List<MapNode> edgeNodes = new List<MapNode>();
        foreach (MapNode node in nodes)
        {
            if (node.posX < thickness || node.posX > width - thickness -1)
            {
                edgeNodes.Add(node);
            } 
            else if (node.posZ < thickness || node.posZ > depth - thickness - 1)
            {
                edgeNodes.Add(node);
            }
        }
        return edgeNodes;
    }

    /*
     * Returns all nodes off the edge of the map according to a thickness value.
     */
    public List<MapNode> GetCenterNodes(int thickness = 1)
    {
        List<MapNode> edgeNodes = new List<MapNode>();
        foreach (MapNode node in nodes)
        {
            if (node.posX > thickness && node.posX < width - thickness - 1)
            {
                if (node.posZ > thickness && node.posZ < depth - thickness - 1)
                {
                    edgeNodes.Add(node);
                }
                
            }
        }
        return edgeNodes;
    }

    #endregion

    #region Utility Methods

    public void SetWalkable(List<MapNode> nodes, bool walkable)
    {
        foreach (MapNode node in nodes)
        {
            node.walkable = walkable;
        }
    }

    #endregion
}