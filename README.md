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
    astar.SetEnd(map.GetNodeAtIndex(10));
    astar.GetPath();

## Updating the map in realtime
You can update the map in realtime by generating new data (MapNodes) first.

    map.GenerateMapData(10, 10, 1);
    
This will automaticly connect all neighbors of all MapNodes. Note that these connects are also made to non-walkable nodes. Setting the walkable status can also be done in realtime to affect future paths. Keep in mind a Map has (width x depth) amount of MapNodes in total.

    map.GetNodeAtIndex(1).walkable = false;
