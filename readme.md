# Godot Experiments

This is a collection of experiments I've done in Godot to either learn how to do something specific or simply confirm that what I want to do is possible. If it's included in here it means I found a way to get what I wanted working to some degree.

High level project notes are included below. More details projects notes can be found in the project directory's readme file.

### sqlite-on-android

Attempt to create a Godot 4.4.1 project using C# that is capable of using SQLite with Android as the target deployment environment.

Worked perfectly on desktop, but ultimately failed with the SQLite DLL not being found when the project was run on Android and throwing an exception. Pivoted to LiteDB which ran flawlessly on first attempt.

### elastic-controls

Attempt to create a draggable Control node with triggerable behaviour based on node position against available travel allowances and elastic snapback behaviour on release.

Written against Godot 4.4.1 using GDScript. Creates a basic scene that could be dropped into a scene.