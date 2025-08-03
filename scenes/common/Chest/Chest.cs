using Godot;
using System;

public partial class Chest : Node2D
{

	private bool _playerCanOpen = false;
	private bool _opened = false;
	[Export]
	public AudioStreamPlayer2D ChestOpen { get; set; }

	public override void _Ready()
	{
		base._Ready();
		Area2D area = GetNode<Area2D>("Area2D");
		area.BodyEntered += OnBodyEntered;
		area.BodyExited += OnBodyExit;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is cool_wizard_001)
		{
			_playerCanOpen = true;
		}
	}

	private void OnBodyExit(Node body)
	{
		if (body is cool_wizard_001)
		{
			_playerCanOpen = false;
		}
	}

	private void OpenChest()
	{
		if (_opened || !_playerCanOpen)
		{
			return;
		}

		AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play("open");
		ChestOpen.Play();

		_opened = true;

		PlayerInventory.IncreaseTurretCount(1);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (Input.IsActionJustPressed("Interact"))
		{
			OpenChest();
		}
	}
}
