using Godot;

public partial class Camera : CharacterBody3D
{
	[Export]
	public int MoveSpeed { get; set; } = 600;
	[Export]
	public int FallSpeed { get; set; } = 60;
	[Export]
	public int JumpSpeed { get; set; } = 300;
	[Export]
	public float RotMult { get; set; } = 2.0f;
	[Export]
	public Marker3D SpawnPoint;

	private Variant _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity");

	private Node3D _pov;
	private SpotLight3D _flashlight;
	private Timer _coyoteTimer;


	private enum _playerState { Unknown, Walk, Jump, Fall }
	private _playerState _player;

	public override void _Ready()
	{
		_pov = GetNode<Node3D>("POV");
		_flashlight = GetNode<SpotLight3D>("POV/Flashlight");
		_coyoteTimer = GetNode<Timer>("CoyoteTimer");
		GlobalPosition = SpawnPoint.GlobalPosition;
		GlobalBasis = SpawnPoint.GlobalBasis;
		FloorBlockOnWall = false;

		_coyoteTimer.OneShot = true;
		_coyoteTimer.Autostart = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput(delta);
		MoveAndSlide();
		// DebugOutput();
	}

	private void GetInput(double delta)
	{
		Velocity = new Vector3(0, Velocity.Y, 0);

		Vector2 input = Input.GetVector("move_right", "move_left", "move_backward", "move_forward");
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
		Velocity += -Transform.Basis.Z * input.Y * MoveSpeed * (float)delta;
		Velocity += -Transform.Basis.X * input.X * MoveSpeed * (float)delta;
		if (!IsOnFloor())
		{
			Velocity += new Vector3(0, -(float)_gravity * (float)delta, 0);
			if (_player == _playerState.Walk || _player == _playerState.Unknown)
			{
				SetPlayerState(_playerState.Fall);
				_coyoteTimer.Start();
			}
		}
		else
		{
			if (_player != _playerState.Walk)
			{
				SetPlayerState(_playerState.Walk);
			}
		}

		if ((IsOnFloor() || !_coyoteTimer.IsStopped()) && Input.IsActionJustPressed("jump"))
		{
			Velocity += new Vector3(0, JumpSpeed * (float)delta, 0);
			SetPlayerState(_playerState.Jump);
		}

		if (Input.IsActionJustPressed("toggle_flashlight"))
		{
			_flashlight.Visible = !_flashlight.Visible;
		}

		if (Velocity.Y < -30)
		{
			Velocity = Vector3.Zero;
			GlobalPosition = SpawnPoint.GlobalPosition;
			GlobalBasis = SpawnPoint.GlobalBasis;
		}
	}

	private void SetPlayerState(_playerState newState)
	{
		if (newState == _player)
		{
			return;
		}
		_player = newState;
	}

	private void DebugOutput()
	{
	}
}
