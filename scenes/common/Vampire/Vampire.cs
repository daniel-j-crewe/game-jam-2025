using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Vampire : CharacterBody2D
{
	[Export]
	public Vector2 SpawnPoint;
	[Export]
	public new float Velocity;
	[Export]
	public VampireWaypoint[] Waypoints;
	private List<Vector2> TargetPositions = new List<Vector2>();
	private Vector2 CurrentTargetPos;
	private Vector2 PreviousPos;
	private AnimatedSprite2D sprite;
	private int CurrentTargetIndex = 0;
	[Export]
	public NavigationAgent2D NavAgent { get; set; }
	[Export]
	public cool_wizard_001 player { get; set; }
	[Export]
	public bool AggroTrackingEnabled { get; set; }
	private bool MunchZone = false;
	double navUpdateTimer = 0;
	double navUpdateTime = 0.2;
	private Node parent;
	private bool queuedForRemoval = false;
	public SignalController signalController;

	public override void _Ready()
	{
		base._Ready();
		parent = this.GetParent();
		GlobalPosition = SpawnPoint;
		TargetPositions = Waypoints.Select(x => x.GlobalPosition).ToList();
		OrganiseTargetPositions();
		PreviousPos = GlobalPosition;
		CurrentTargetPos = TargetPositions[0];
		NavAgent.TargetPosition = player.GlobalPosition;
		signalController = GetNode<SignalController>("/root/MainSceneRoot/SignalController");
		signalController.ResetLoop += AcknowledgeResetLoop;

		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite");
		sprite.Play("idle");
	}

	public void AcknowledgeResetLoop()
	{
		AggroTrackingEnabled = false;
		GlobalPosition = SpawnPoint;
		CurrentTargetPos = TargetPositions[0];
		RestoreToMap();
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (queuedForRemoval)
		{
			queuedForRemoval = false;
			if (parent.GetChildren().Contains(this)) parent.RemoveChild(this);
			return;
		}

		navUpdateTimer -= delta;

		if (navUpdateTimer < 0)
		{
			navUpdateTimer = navUpdateTime;
			NavAgent.TargetPosition = player.GlobalPosition;
		}

		Vector2 newPos;

		if (AggroTrackingEnabled)
		{
			newPos = GlobalPosition.MoveToward(NavAgent.GetNextPathPosition(), Velocity * (float)delta);
		}
		else
		{
			newPos = GlobalPosition.MoveToward(CurrentTargetPos, Velocity * (float)delta);
		}

		Vector2 MovementDifferential = GlobalPosition - PreviousPos;

		if (MunchZone)
		{
			sprite.Play("attack");
		}
		else if (MovementDifferential.LengthSquared() > 0.01f)
		{
			if (sprite.Animation != "walk")
				sprite.Play("walk");
		}
		else
		{
			if (sprite.Animation != "idle")
				sprite.Play("idle");
		}

		if (Mathf.Abs(MovementDifferential.X) > 0.01f)
		{
			sprite.FlipH = MovementDifferential.X < 0;
		}

		PreviousPos = GlobalPosition;
		GlobalPosition = newPos;

		if (Position == CurrentTargetPos)
		{
			CurrentTargetPos = GetNextTargetPos();
		}
	}

	public void VisionZoneEntered()
	{
		AggroTrackingEnabled = true;
	}

	public void MunchZoneToggle(bool updatedBool = false)
	{
		MunchZone = updatedBool;
	}

	public void HitAndRemove()
	{
		queuedForRemoval = true;
	}

	public void RestoreToMap()
	{
		if (!parent.GetChildren().Contains(this)) parent.AddChild(this);
	}

	private void OrganiseTargetPositions()
	{
		if (TargetPositions.Count == 0)
		{
			TargetPositions.Add(SpawnPoint);
		}
		else
		{
			TargetPositions.Reverse();
			List<Vector2> returnPath = [.. TargetPositions];
			TargetPositions.Reverse();
			TargetPositions.AddRange(returnPath);
			TargetPositions.Add(SpawnPoint);
		}
	}

	private Vector2 GetNextTargetPos()
	{
		CurrentTargetIndex++;
		if (CurrentTargetIndex > TargetPositions.Count - 1)
		{
			CurrentTargetIndex = 0;
		}
		return TargetPositions[CurrentTargetIndex];
	}
}
