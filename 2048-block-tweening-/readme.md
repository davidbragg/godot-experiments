# walking_sim

### Objective

Test out tweening with `Sprite2D` nodes to test some basic ideas for an upcoming 2048 style game. Using C# with Android as the target export environment.

Tween should scale up in a ballooning effect and then shrink back to original size, with an elastic effect if possible, and a colour change.

### Outcome

Getting the tweens I wanted required being able to offset the position the tween started at. This is possible with `Control` nodes. I'm using a `Control` node as the base for a tile and this appears to work fine for my purposes.

### Additional Notes

#### Initial Notes

The theoretical max of a 4x4 configuration of 2048 is either 2<sup>16</sup> if the max new tile is a 2 or 2<sup>17</sup> if a new tile can be a 4. With that in mind, I'll need a total of 17 colours for tiles, but that is beyond the scope of this experiment. I just need a few for this instance.

Given that I'm not planning on having values in my tiles, as I'm following the now-dead game `Hues` and using tile colour to denote value, I don't need to actually track a value. I should be able to use an incrementing value, starting at `0`. This can then double as the index for the colour value to pull from the array of colours for a tile.

Again, beyond the scope of this particular experiment, but worth noting down for the future.

#### Project Setup

The target export environment is Android. I wasn't entirely clear on the best way to approach this. Luckily, there's a section in the Godot docs that functionally covers what I need.

[Godot Docs: Multiple resolutions/Mobile game in portrait mode](https://docs.godotengine.org/en/stable/tutorials/rendering/multiple_resolutions.html#mobile-game-in-portrait-mode)

The recommendation here is to set the window to `720 x 1280`. Alternatively `1080 x 1920`. I'll need to consider what I actually want out of this and if the SVGs scale appropriately to avoid annoyance.

* set `stretch mode` > `canvas_items`
* set `stretch aspect` > `expand`
* set `Display` > `Window` > `Handheld` > `Orientation` > `Portrait`
* configure Control nodes' anchors to snap to the correct corners using the `Layout` menu

I copied over the `.vscode` directory from the `sqlite on android` project. This gave me all the necessary functionality to build/debug from vs code.

The final step was to set up an export target to Android. Once that was in place, I was able to get it building and debugging on my phone over USB.

#### Testing Colour Modulation

I initially started with a simple `Polygon2D`. The process shouldn't be any different on a tween against `modulate` parameter.

```cs
tween.TweenProperty(tile, "modulate", colors[index], 0.15f);
```

This functionally covers what's needs to modulate the colour of the node.

I created a `color` array to store some hex colours.

```cs
private Color[] colors = [
	// green
	new Color(0x00ff00ff),
	// red
	new Color(0xff0000ff),
	// blue
	new Color(0x0000ffff),
	// black
	new Color(0x000000ff)
];
```

Switching over to a `Sprite2D` didn't change anything here.

#### Scale tweening

Because I needed to scale from the middle of the node, I ended up using a `Control` node as the parent for this functionality. It has a `pivot offset` parameter that can then be set to the center of the node. This then translates to the tile scaling out from the center instead of from the left hand corner.

The entire tween process looks like this.

```cs
// create the new tween
Tween tween = GetTree().CreateTween();
// set the transition type
tween.SetTrans(Tween.TransitionType.Circ);
// scale the node up by 1.1 on the x & y
tween.TweenProperty(this, "scale", new Vector2(1.1f, 1.1f), 0.2f);
// tween the tile color from the current value to the value at `color[index]`
tween.TweenProperty(tile, "modulate", colors[index], 0.15f);
// simultaneously scale the node down to the original size
tween.Parallel().TweenProperty(this, "scale", new Vector2(1.0f, 1.0f), 0.15f);
```

#### Mobile Sizing

Despite setting up my project as initially noted, I was getting some issues on my phone. The display area ratio is taller than 9:16. With the way it was set up I was left anchored to the top left of the screen and the `ColorRect` I was using along the background was stopping part way down the screen.

I attempted several fixes as suggested by a quick search. All the answers were pointing the same direciton, implying my project setup should resolve this. Rather than continue to search I brought a `Camera2D` in to appropriately center the view and then extended the `ColorRect` past the boundaries of the project to accomodate addtionial screen ratios.

It's not an ideal solution, but it works in context.
