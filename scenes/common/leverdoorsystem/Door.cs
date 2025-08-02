using Godot;
using System;

public partial class Door : StaticBody2D
{
	[Export]
	public CollisionShape2D Hitbox { get; set; }

	private AnimatedSprite2D _sprite;
	private bool _isOpen = false;

	public override void _Ready()
	{
		base._Ready();
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public void FlipState()
	{
		_isOpen = !_isOpen;
		this.Hitbox.Disabled = _isOpen;
		_sprite.Play(_isOpen ? "open" : "close");
	}
}
