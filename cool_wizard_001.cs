using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class cool_wizard_001 : CharacterBody2D
{
	Vector2 velocity;
	[Export]
	public float Speed { get; set; }
	[Export]
	public AnimatedSprite2D AnimatedSprite2D { get; set; }
	[Export]
	public TileMapLayer map { get; set; }
	public SignalController signalController;
	[Export]
	public Vector2 StartPos;
	[Export]
	public double SpawnedTowerMovementVelocity;
	[Export]
	public double SpawnedTowerTimeToLive;
	[Export]
	public double SpawnedTowerRateOfFire;
	public DoorLever CurrentDoorLever { get; set; }
	public TeleporterStartNode CurrentTeleporterStartNode { get; set; }
	public TeleporterEndNode CurrentTeleporterEndNode { get; set; }
	private bool TurretPlaceModeActive = false;
	private bool blockDefaultAnims = false;
	private bool validPlacementLocation;
	private Vector2 placementPostion;
	[Export]
	public string[] DeathTexts;
	private List<string> deathTextsList = new List<string>();

	[Export]
	public TextBubble textBubble { get; set; }


	[Export]
	public TurretPlacementIndicator PlacementIndicator { get; set; }

	public override void _Ready()
	{
		base._Ready();
		GlobalPosition = StartPos;

		signalController = GetNode<SignalController>("/root/MainSceneRoot/SignalController");
		signalController.ResetLoop += AcknowledgeResetLoop;
		deathTextsList = DeathTexts.ToList();
		RandomiseDeathList();

		AnimatedSprite2D.AnimationFinished += ClearBlockAnims;
		AnimatedSprite2D.Play("idle");
	}

	private void ClearBlockAnims()
	{
		if (blockDefaultAnims)
		{
			blockDefaultAnims = false;
		}
	}

	public void SetPlayerText(string text)
	{
		textBubble.SetText(text);
	}

	private string GetNextDeathText()
	{
		string nextText = deathTextsList.FirstOrDefault();
		if (string.IsNullOrEmpty(nextText))
		{
			deathTextsList = DeathTexts.ToList();
			RandomiseDeathList();
			nextText = deathTextsList.FirstOrDefault();
		}
		deathTextsList.Remove(nextText);
		return nextText;
	}

	private void RandomiseDeathList()
	{
		Random rnd = new Random();
		deathTextsList = deathTextsList.OrderBy<string, int>((item) => rnd.Next()).ToList();
	}

	private static bool IsPlayerMoving()
	{
		Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
		return direction.LengthSquared() > 0.01f;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		PlacementIndicator.Visible = TurretPlaceModeActive;

		if (TurretPlaceModeActive)
		{
			Vector2 mouseLocation = map.GetLocalMousePosition();
			Vector2I mapIndex = map.LocalToMap(mouseLocation);
			TileData data = map.GetCellTileData(mapIndex);
			if (data != null)
			{
				validPlacementLocation = (bool)data.GetCustomData("TurretPlaceable");
				placementPostion = mouseLocation;
				GD.Print($"{mapIndex} : {data.GetCustomData("TurretPlaceable")}");
				PlacementIndicator.GlobalPosition = mouseLocation;
				PlacementIndicator.isCurrentlyValid = validPlacementLocation;
			}
		}

		if (Input.IsActionJustPressed("Interact"))
		{
			if (CurrentDoorLever != null)
			{
				CurrentDoorLever.PullLever();
			}
			if (CurrentTeleporterStartNode != null)
			{
				if (CurrentTeleporterStartNode.IsActive && CurrentTeleporterStartNode.LinkedEndNode != null)
				{
					this.GlobalPosition = CurrentTeleporterStartNode.LinkedEndNode.GlobalPosition;
				}
			}
			if (CurrentTeleporterEndNode != null)
			{
				if (!CurrentTeleporterEndNode.IsActive)
				{
					CurrentTeleporterEndNode.SetActive();
				}
				else if (CurrentTeleporterEndNode.IsActive && CurrentTeleporterEndNode.LinkedStartNode != null)
				{
					this.GlobalPosition = CurrentTeleporterEndNode.LinkedStartNode.GlobalPosition;
				}
			}
		}


		if ((Input.IsActionJustPressed("PlaceTurret") || (Input.IsActionJustPressed("ConfirmPlaceTurret") && TurretPlaceModeActive)) && PlayerInventory.TurretCount > 0)
		{
			if (TurretPlaceModeActive && validPlacementLocation)
			{
				PackedScene packedScene = ResourceLoader.Load("res://scenes/common/CrystalTurret/crystal_turret.tscn") as PackedScene;
				CrystalTurret turret = packedScene.Instantiate() as CrystalTurret;
				turret.MovementVelocity = SpawnedTowerMovementVelocity;
				turret.TimeToLive = SpawnedTowerTimeToLive;
				turret.RateOfFire = SpawnedTowerRateOfFire;
				turret.GlobalPosition = placementPostion;
				map.AddChild(turret);
				PlayerInventory.DecreaseTurretCount();

				blockDefaultAnims = true;
				AnimatedSprite2D.Play("wack");
				TurretPlaceModeActive = false;
				GD.Print($"placed turret {placementPostion}");
			}
			else
			{
				TurretPlaceModeActive = true;
			}
		}

		if (Input.IsActionJustPressed("SpaceBar"))
		{
			blockDefaultAnims = true;
			AnimatedSprite2D.Play("awooga");
		}

		if (Input.IsActionJustPressed("CancelPlaceTurret"))
		{
			TurretPlaceModeActive = false;
		}

		Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
		velocity.X = direction.X * Speed;
		velocity.Y = direction.Y * Speed;
		Velocity = velocity;
		MoveAndSlide();

		if (direction.X != 0)
		{
			AnimatedSprite2D.FlipH = direction.X < 0;
		}

		if (!blockDefaultAnims)
		{
			if (IsPlayerMoving())
			{
				AnimatedSprite2D.Play("walk");
			}
			else
			{
				AnimatedSprite2D.Play("idle");
			}
		}
	}

	public void AcknowledgeResetLoop()
	{
		GlobalPosition = StartPos;
		SetPlayerText(GetNextDeathText());
	}

}
