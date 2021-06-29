# Astar
An A* implementation on a X/Z grid.

# Making a new A* grid

map = new Map(100, 100, 1.0f);
astar = new Astar();
astar.SetMap(map);

# Getting a path from the A* grid

astar.SetStart(map.GetNodeAtIndex(0));
astar.SetEnd(map.GetNodeAtIndex(map.nodes.Length -2));
astar.GetPath();
