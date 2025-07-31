using Godot;
using System;

public partial class CrystalTurret : StaticBody2D
{
    [Export]
    public double RateOfFire;
    private double rateCountdown;
    [Export]
    public double MovementVelocity;
    [Export]
    public double TimeToLive;
    [Export]
    public Vector2[] DirectionsToMoveIn;
    [Export]
    public Node2D Holder;
    private Node Parent;
    private bool isActive;
    private Enemy CurrentTargetEnemy;


    public override void _Ready()
    {
        base._Ready();
        Parent = this.GetParent();
    }


    public override void _Process(double delta)
    {
        base._Process(delta);
        if (isActive && CurrentTargetEnemy != null)
        {
            rateCountdown -= delta;
            if (rateCountdown <= 0)
            {
                rateCountdown = RateOfFire;
                PackedScene packedScene = ResourceLoader.Load("res://scenes/common/CrystalTurret/crystal_bullet.tscn") as PackedScene;
                CrystalBullet bullet = packedScene.Instantiate() as CrystalBullet;
                bullet.MovementVelocity = MovementVelocity;
                bullet.DirectionToMoveIn = this.GlobalPosition.DirectionTo(CurrentTargetEnemy.GlobalPosition);
                bullet.TimeToLive = TimeToLive;
                Holder.AddChild(bullet);
                this.LookAt(GlobalPosition + this.GlobalPosition.DirectionTo(CurrentTargetEnemy.GlobalPosition));
                Holder.RotationDegrees = -1 * this.RotationDegrees;
            }
        }
    }

    public void PlayerEnteredActivationZone()
    {
        this.isActive = true;
    }

    public void PlayerLeftActivationZone()
    {
        this.isActive = false;
    }

}
