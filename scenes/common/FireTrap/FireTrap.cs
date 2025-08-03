using Godot;
using System;
using System.Linq;

public partial class FireTrap : Node2D
{
	[Export] public float ActivationInterval = 1.0f;
	[Export] public float InitialDelay = 0.0f;
	[Export] public string ActivationAnimation = "fire_cycle";
	[Export] public int[] OuchieFrames { get; set; } = [2, 3];
	[Export] public AnimatedSprite2D Sprite { get; set; }
	[Export] public Area2D Area { get; set; }
	[Export] public Timer Timer { get; set; }

	private bool BigOuchiePossible = false;
	private bool PlayerInDangerZone = false;
	public SignalController signalController;

	[Export] public AudioStreamPlayer2D TrapSound { get; set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		string ErrPrefix = "fire_trap: ";

		if (Sprite == null)
		{
			GD.PushError(ErrPrefix + "Sprite Not Defined");
		}

		if (Area == null)
		{
			GD.PushError(ErrPrefix + "Area Not Defined");
		}

		if (Timer == null)
		{
			GD.PushError(ErrPrefix + "Timer Not Defined");
		}

		Timer.WaitTime = ActivationInterval + InitialDelay;
		Timer.Timeout += PlayAnim;
		Timer.Start();

		Area.BodyEntered += OnBodyEntered;
		Area.BodyExited += OnBodyExited;

		Sprite.FrameChanged += CheckIsDangerousFrame;

		signalController = GetNode<SignalController>("/root/MainSceneRoot/SignalController");
	}

	private void PlayAnim()
	{
		Sprite.Play(ActivationAnimation);
		Timer.WaitTime = ActivationInterval;
		TrapSound.Play();
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is cool_wizard_001)
			PlayerInDangerZone = true;
	}

	private void OnBodyExited(Node2D body)
	{
		if (body is cool_wizard_001)
			PlayerInDangerZone = false;
	}

	private void CheckIsDangerousFrame()
	{
		BigOuchiePossible = OuchieFrames.Contains(Sprite.Frame);

		if (BigOuchiePossible && PlayerInDangerZone)
		{
			GD.Print("Pain inflicted");
			signalController.EmitSignal("ResetLoop");
		}
	}
}
