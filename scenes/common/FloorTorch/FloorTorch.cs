using Godot;
using System;

public partial class FloorTorch : Node2D
{
	private AnimatedSprite2D _animationPlayer;
	private Timer _timer;
	private readonly Random _random = new();

	public override void _Ready()
	{
		base._Ready();
		_animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_timer = GetNode<Timer>("Timer");

		_timer.Timeout += OnTimerTimeout;

		ResetTimer();
	}
	
	private void OnTimerTimeout()
	{
		_animationPlayer.Play("torchmode");
		ResetTimer();
	}

	private void ResetTimer()
	{
		_timer.WaitTime = _random.Next(1, 9); 
		_timer.Start();
	}
}
