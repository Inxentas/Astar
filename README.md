# Astar
A relatively simple A* implementation on a X/Z grid.

## Making a new A* grid
Both the Map as the Astar classes need to be instantiated first. Astar encapsulates the map like this:

    Map map = new Map(100, 100, 1.0f);
    Astar astar = new Astar();
    astar.SetMap(map);

## Getting a path from the A* grid
Once the Astar / Map has been instantiated, paths can be requested like this:

    astar.SetStart(map.GetNodeAtIndex(0));
    astar.SetEnd(map.GetNodeAtIndex(map.nodes.Length -2));
    astar.GetPath();
