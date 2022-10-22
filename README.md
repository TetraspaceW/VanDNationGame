# VanDNationGame
Zoom in, zoom out.

![image](https://user-images.githubusercontent.com/2828594/197349263-f78a4c8f-e6d8-4953-90fa-ee6a4c741a2d.png)

This game is a universe generator. Each map is 10×10 tiles, and clicking on a tile expands it into a full map of its own representing a region of space ten times smaller. The scale of the game ranges from a 10 billion light year universe map to a 1 attometer quark map.

## Installation

The game is made in Godot Mono 3.4.4, which can be downloaded at https://godotengine.org/download/windows.

For C#, the .NET SDK is needed, which can be downloaded at https://dotnet.microsoft.com/en-us/download. 

Once Godot Mono and the .NET SDK are set up, the project can be imported into Godot by opening Godot, choosing "Import", and then importing the `project.godot`. Depending on how Mono was set up, Godot might not like building it straight away; if it gives a strange error like "build tools not found", check to make sure the .NET SDK in installed and otherwise in the Godot editor try Editor > Editor Settings > Mono > Builds and choosing a different build tool.

Things that would be nice to have in the game are in the Github issues https://github.com/TetraspaceW/VanDNationGame/issues.
