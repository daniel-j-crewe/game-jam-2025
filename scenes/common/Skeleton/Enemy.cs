using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public Vector2 StartPos;
	[Export]
	public double InitialAggroDisableTime = 0.5;
	private double savedAggroTime;
	[Export]
	public float Velocity;
	public Vector2[] TargetPosArray;
	[Export]
	public EnemyWaypoint[] Waypoints;
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
	double navUpdateTimer = 0;
	double navUpdateTime = 0.2;
	private Node parent;
	private bool BoneZone = false;
	private bool queuedForRemoval = false;



	public SignalController signalController;

	public override void _Ready()
	{
		base._Ready();
		parent = this.GetParent();
		GlobalPosition = StartPos;
		TargetPositions = Waypoints.Select(x => x.GlobalPosition).ToList();
		OrganiseTargetPositions();
		PreviousPos = GlobalPosition;
		CurrentTargetPos = TargetPositions[0];
		NavAgent.TargetPosition = player.GlobalPosition;
		signalController = GetNode<SignalController>("/root/MainSceneRoot/SignalController");
		signalController.ResetLoop += AcknowledgeResetLoop;
		savedAggroTime = InitialAggroDisableTime;
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play("idle");

	}

	public void AcknowledgeResetLoop()
	{
		AggroTrackingEnabled = false;
		GlobalPosition = StartPos;
		CurrentTargetPos = TargetPositions[0];
		RestoreToMap();
		InitialAggroDisableTime = savedAggroTime;
	}

	public void BoneZoneToggle(bool updatedBool = false)
	{
		BoneZone = updatedBool;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (!(InitialAggroDisableTime < 0))
			InitialAggroDisableTime -= delta;
		if (queuedForRemoval)
		{
			queuedForRemoval = false;
			if (this.GetParent() != null && this.GetParent().GetChildren().Contains(this)) parent.RemoveChild(this);
			return;
		}
		navUpdateTimer -= delta;
		if (AggroTrackingEnabled) GD.Print("aggroed");

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

		if (BoneZone)
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
		if (InitialAggroDisableTime < 0)
			AggroTrackingEnabled = true;
	}

	public void HitAndRemove()
	{
		queuedForRemoval = true;
	}

	public void RestoreToMap()
	{
		if (!parent.GetChildren().Contains(this) && this.GetParent() == null) parent.CallDeferred("add_child", this);
	}

	private void OrganiseTargetPositions()
	{
		if (TargetPositions.Count == 0)
		{
			TargetPositions.Add(StartPos);
		}
		else
		{
			TargetPositions.Reverse();
			List<Vector2> returnPath = [.. TargetPositions];
			TargetPositions.Reverse();
			TargetPositions.AddRange(returnPath);
			TargetPositions.Add(StartPos);
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
