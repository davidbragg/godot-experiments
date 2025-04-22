# Game Dev & Godot Experiments

## Projects

This is a collection of experiments I've done in Godot to either learn how to do something specific or simply confirm that what I want to do is possible. If it's included in here it means I found a way to get what I wanted working to some degree.

High level project notes are included below. More detailed project notes can be found in the project directory's readme file.

### 2048-block-tweening-

A quick experiment for tweening `Sprite2D` nodes on Android touch input.

Written against Godot 4.4.1 using C#.

### elastic-controls

Attempt to create a draggable Control node with triggerable behaviour based on node position against available travel allowances and elastic snapback behaviour on release.

Written against Godot 4.4.1 using GDScript. Creates a basic scene that could be dropped into a scene.

### sqlite-on-android

Attempt to create a Godot 4.4.1 project using C# that is capable of using SQLite with Android as the target deployment environment.

Worked perfectly on desktop, but ultimately failed with the SQLite DLL not being found when the project was run on Android and throwing an exception. Pivoted to LiteDB which ran flawlessly on first attempt.

## Polyglot Notebooks

Found in the polyglot-notebooks folder, these are notebooks that are a combination of markdown and C# that allow running snippets of C# code in vs code. I use these to explore technical concepts or to solve implementation problems without the burden of full projects.

### 2048_exploration

This is an exploration of how to manage a game of 2048 in C# code independently. My goal was to attempt to break down the game logic into functional pieces without looking up prior implementations. It is an attempt to encapsulate the game state and game manager in a single class.