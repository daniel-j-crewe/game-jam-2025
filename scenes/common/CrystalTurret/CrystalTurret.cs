using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
    private List<Enemy> EnemiesInRange = new List<Enemy>();

    public SignalController signalController;


    public override void _Ready()
    {
        base._Ready();
        Parent = this.GetParent();
        signalController = GetNode<SignalController>("/root/MainSceneRoot/SignalController");
        signalController = GetNode<SignalController>("/root/MainSceneRoot/SignalController");

        signalController.ResetLoop += AcknowledgeResetLoop;

    }
    public void AcknowledgeResetLoop()
    {
        foreach (CrystalBullet bullet in Holder.GetChildren().Where(x => x is CrystalBullet))
        {
            bullet.QueueFree();
        }
    }


    public override void _Process(double delta)
    {
        base._Process(delta);
        if (isActive && CurrentTargetEnemy != null)
        {
            rateCountdown -= delta;
            if (rateCountdown <= 0)
            {
                DetermineTarget();
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

    private void DetermineTarget()
    {
        Enemy closestEnemy = null;
        closestEnemy = EnemiesInRange.OrderBy(x => x.GlobalPosition.DistanceTo(this.GlobalPosition)).FirstOrDefault();
        CurrentTargetEnemy = closestEnemy;
        GD.Print($"DeterminedTarget {closestEnemy == null}");

    }

    public void PlayerEnteredActivationZone()
    {
        this.isActive = true;
        GD.Print("player entered zone");
    }

    public void PlayerLeftActivationZone()
    {
        this.isActive = false;
        GD.Print("player left zone");
    }

    public void EnemyEnteredActivationZone(Enemy enemy)
    {
        if (!EnemiesInRange.Contains(enemy)) EnemiesInRange.Add(enemy);
        DetermineTarget();
        GD.Print("enemy entered zone");
    }

    public void EnemyLeftActivationZone(Enemy enemy)
    {
        if (EnemiesInRange.Contains(enemy)) EnemiesInRange.Remove(enemy);
        DetermineTarget();
        GD.Print("enemy left zone");
    }

}
