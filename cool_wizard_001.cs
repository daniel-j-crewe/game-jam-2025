using Godot;
using System;

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
    public DoorLever CurrentDoorLever { get; set; }
    private bool TurretPlaceModeActive = false;
    private bool validPlacementLocation;
    private Vector2 placementPostion;

    public override void _Ready()
    {
        base._Ready();
        AnimatedSprite2D.Play();
        signalController = GetNode<SignalController>("/root/MainSceneRoot/SignalController");
        signalController.ResetLoop += AcknowledgeResetLoop;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

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

            }
        }

        if (Input.IsActionJustPressed("Interact"))
        {
            if (CurrentDoorLever != null)
            {
                CurrentDoorLever.PullLever();
            }
        }


        if (Input.IsActionJustPressed("PlaceTurret") && PlayerInventory.TurretCount > 0)
        {
            if (TurretPlaceModeActive && validPlacementLocation)
            {
                PackedScene packedScene = ResourceLoader.Load("res://scenes/common/CrystalTurret/crystal_turret.tscn") as PackedScene;
                CrystalTurret turret = packedScene.Instantiate() as CrystalTurret;
                turret.MovementVelocity = SpawnedTowerMovementVelocity;
                turret.TimeToLive = SpawnedTowerTimeToLive;
                turret.GlobalPosition = placementPostion;
                map.AddChild(turret);
                PlayerInventory.DecreaseTurretCount();
                GD.Print($"placed turret {placementPostion}");
            }
            else
            {
                TurretPlaceModeActive = true;
            }
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
    }

    public void AcknowledgeResetLoop()
    {
        GlobalPosition = StartPos;
    }

}
