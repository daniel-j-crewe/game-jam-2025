using Godot;

[Tool]
public partial class WallTorch : Node2D
{
	private AnimatedSprite2D _sprite;
	public enum DirectionOptions
	{
		Left,
		Right
	};

	private DirectionOptions _directionToFace = DirectionOptions.Left;
	[Export]
	public DirectionOptions TorchDirection
	{
		get => _directionToFace;
		set
		{
			_directionToFace = value;
			UpdateDirection();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		UpdateDirection();

		// don't animate in editor thanks :)
		if (!Engine.IsEditorHint())
		{
			_sprite.Play("default");
		}
		
	}

	private void UpdateDirection()
	{
		if (_sprite == null)
		{
			return;
		}

		_sprite.FlipH = _directionToFace != DirectionOptions.Left;
	}
}
