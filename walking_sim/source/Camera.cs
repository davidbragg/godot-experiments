using Godot;
using System;

public partial class Camera : CharacterBody3D
{
	[Export]
	public int Speed { get; set; } = 6;
	[Export]
	public int FallAcceleration { get; set; } = 75;
	[Export]
	public float RotMult { get; set; } = 2.0f;

	private Vector3 _targetVelocity = Vector3.Zero;
	private Node3D _pov;

	public override void _Ready()
	{
		_pov = GetNode<Node3D>("POV");
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput(delta);

		MoveAndSlide();
	}

	private void GetInput(double delta)
	{
		Velocity = Vector3.Zero;
		float move = Input.GetAxis("move_backward", "move_forward");
		float step = Input.GetAxis("move_right", "move_left");
		float turn = Input.GetAxis("look_right", "look_left");
		float look = Input.GetAxis("look_down", "look_up");

		// rotate the camera & light on the X axis, independently of the player
		_pov.RotateX(RotMult * look * (float)delta);
		float lookClamp = Mathf.Pi / 2.6f;
		float x = Mathf.Clamp(_pov.Rotation.X, -lookClamp, lookClamp);
		float y = _pov.Rotation.Y;
		float z = _pov.Rotation.Z;
		_pov.Rotation = new Vector3(x, y, z);

		// rotate the player on the Y axis & move along the rotated X & Z
		RotateY(RotMult * turn * (float)delta);
		Velocity += -Transform.Basis.Z * move * Speed;
		Velocity += -Transform.Basis.X * step * Speed;

		if (Input.IsActionJustReleased("toggle_flashlight"))
		{
			GetNode<SpotLight3D>("POV/Flashlight").Visible = !GetNode<SpotLight3D>("POV/Flashlight").Visible;
		}
	}
}
