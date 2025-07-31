using Godot;
using System;
using System.Linq;

public partial class SpikeTrap : Node2D
{
	[Export] public float ActivationInterval = 1.0f;
	[Export] public string ActivationAnimation = "spike_cycle";
	[Export] public int[] OuchieFrames { get; set; } = [2, 3];
	[Export] public AnimatedSprite2D Sprite { get; set; }
	[Export] public Area2D Area { get; set; }
	[Export] public Timer Timer { get; set; }

	private bool BigOuchiePossible = false;
	private bool PlayerInDangerZone = false;
	public SignalController signalController;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		string ErrPrefix = "spike_trap: ";

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

		Timer.WaitTime = ActivationInterval;
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
	}

	private void OnBodyEntered(Node2D body)
	{
		PlayerInDangerZone = true;
	}

	private void OnBodyExited(Node2D body)
	{
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
