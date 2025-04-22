using Godot;

public partial class Tile : Control
{
	private Color[] colors = [
		new Color(0x00ff00ff),
		new Color(0xff0000ff),
		new Color(0x0000ffff),
		new Color(0x000000ff)
	];
	private int index = 0;
	private Sprite2D tile;

	public override void _Ready()
	{
		tile = GetNode<Sprite2D>("Tile");
	}

	public void GuiInput(InputEvent inputEvent)
	{
		// check if we're getting a left mousebutton pressed event
		// doubles for screen touch
		if (
			inputEvent is InputEventMouseButton mouseEvent
			&& mouseEvent.Pressed
			&& mouseEvent.ButtonIndex == MouseButton.Left
		)
		{
			Tween tween = GetTree().CreateTween();
			tween.SetTrans(Tween.TransitionType.Circ);
			tween.TweenProperty(this, "scale", new Vector2(1.1f, 1.1f), 0.2f);
			tween.TweenProperty(tile, "modulate", colors[index], 0.15f);
			tween.Parallel().TweenProperty(this, "scale", new Vector2(1.0f, 1.0f), 0.15f);
			// iterate index for colors
			index++;
			if (index == colors.Length)
			{
				index = 0;
			}
		}
	}
}
