using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapNode
{
    [Tooltip("The grid position X value.")]
    public int posX;
    [Tooltip("The grid position Z value.")]
    public int posZ;
    [Tooltip("The Map and MapNode scale.")]
    public float scale;

    [Tooltip("Name of the MapNode. Just for reference, it should not be evaluated by code.")]
    public string name;
    [Tooltip("The realscale world position.")]
    public Vector3 position;
    [Tooltip("Determines whether the node is eligable for Astar pathfinding.")]
    public bool walkable;
    [Tooltip("The GameObject (if any) associated with this node.")]
    public GameObject gameobject;

    [Header("Neighbors")]
    public MapNode left;
    public MapNode right;
    public MapNode back;
    public MapNode forward;

    [Header("A* Pathfinding")]
    public MapNode parentNode = null;
    public float G; // the movement cost to move from the starting point to a given square on the grid, following the path generated to get there. 
    public float H; // the estimated movement cost to move from that given square on the grid to the final destination.
    public float F; // The sum of G and H

    public MapNode(int posX, int posZ, float scale = 1.0f)
    {
        this.posX = posX;
        this.posZ = posZ;
        this.scale = scale;
        this.position = new Vector3(posX, 0, posZ) * scale;
        this.walkable = true;
        this.name = "MapNode_" + posX + "_" + posZ;
    }

    public MapNode[] GetNeighbors()
    {
        List<MapNode> list = new List<MapNode>();

        if (left != null) { list.Add(left); }
        if (right != null) { list.Add(right); }
        if (back != null) { list.Add(back); }
        if (forward != null) { list.Add(forward); }

        MapNode[] neighbors = new MapNode[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            neighbors[i] = list[i];
        }

        return neighbors;
    }

    #region Mesh Generation

    public Vector3[] GetVertices()
    {
        Vector3[] v = new Vector3[6];
        v[0] = new Vector3(0, 0, 0);
        v[1] = new Vector3(0, 0, scale);
        v[2] = new Vector3(scale, 0, scale);
        v[3] = new Vector3(scale, 0, scale);
        v[4] = new Vector3(scale, 0, 0);
        v[5] = new Vector3(0, 0, 0);
        // apply scale offset
        for (int i = 0; i < v.Length; i++)
        {
            v[i].x -= (scale * 0.5f);
            v[i].z -= (scale * 0.5f);
        }
        return v;
    }

    public int[] GetTriangles()
    {
        int[] triangles = new int[6] { 0, 1, 2, 3, 4, 5 };
        return triangles;
    }

    public Vector3[] GetNormals()
    {
        Vector3[] normals = new Vector3[6] { Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        return normals;
    }

    #endregion
}
