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

![](embed/facetween.mp4)



