# in-the-shadows
In the shadows is a videogame project. The original task was created in School 42. I used Godot as my engine of choice with C# as the programming language, to refresh my Godot knowledge and to discover and learn C#.

## Game synopsis
You need to rotate and move around different 3D objects in order to make their shadows appear as the the silhouettes that are asked for in each level. Of course, the level names are quite cryptic to make the task a little less obvious.

## Technical side
The game currently features six levels. However, adding levels is quite easy. You add a name to the array located in the Settings script, you create a 3D model with a particular silhouette and then you place it in the main 3D hub for containing the levels, assigning the correct rotations required for solving the level and the possible symmetry. You can also create a level with multiple models, which will require you to enter the offset required between them. Then you need to create a level button in the level selector scene (*BeautifulLevelMenu.tscn*) and position it as you see fit. Done!

There's additional debug info which can be enabled by ticking on the visibility of the "debug label" in the main 3D scene. You can demonstrate anything to your liking there, with the default being the object offset.

The game heavily utilizes what would be Godot Signals, which are Events in C#. Keep in mind that if you add some signal of your own, you must remember to disconenct it with the ```-=``` operator in the (for example) ```_ExitTree``` function of the node you connected it in, so that it disappears on destruction and doesn't interfere with the process when you want to re-connect it later. Not doing so may cause very strange bugs that are seemingly impossible to track down.
