using Godot;
using System;
using System.Linq;

public partial class SpikeTrap : Node2D
{
	[Export] public int[] OuchieFrames { get; set; } = [2, 3];
	[Export] public float ActivationInterval = 1.0f;
	[Export] public string ActivationAnimation = "spike_cycle";
	[Export] public AnimatedSprite2D Sprite { get; set; }
	[Export] public Area2D Area { get; set; }
	[Export] public Timer Timer { get; set; }

	private bool BigOuchiePossible = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		string ErrPrefix = "spike_trap: ";

		if (Sprite == null)
		{
			GD.PushError(ErrPrefix + "Sprite Not Defined");
		}

		if (Timer == null)
		{
			GD.PushError(ErrPrefix + "Timer Not Defined");
		}

		Timer.WaitTime = ActivationInterval;
		Timer.Start();

		Area.BodyEntered += OnPlayerEnter;

		Timer.Timeout += PlayAnim;
		Sprite.FrameChanged += CheckIsDangerousFrame;

	}

	private void PlayAnim()
	{
		Sprite.Play(ActivationAnimation);
	}

	private void CheckIsDangerousFrame()
	{
		BigOuchiePossible = OuchieFrames.Contains(Sprite.Frame);
	}

	private void OnPlayerEnter(Node body)
	{
		GD.Print(BigOuchiePossible);
		if (BigOuchiePossible)
		{
			GD.Print("OOOOOWWWWWIEEEE");
		}
	}
}
