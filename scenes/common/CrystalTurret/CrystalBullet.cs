using Godot;
using System;

public partial class CrystalBullet : Area2D
{
    public double MovementVelocity;
    public Vector2 DirectionToMoveIn;
    [Export]
    public double TimeToLive;
    private double timeAlive;
    Vector2 TargetPos;
    public override void _Ready()
    {
        base._Ready();
        TargetPos = new Vector2(10000, 10000) * DirectionToMoveIn;
        this.LookAt(GlobalPosition + DirectionToMoveIn);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        timeAlive += delta;
        if (timeAlive >= TimeToLive)
        {
            this.QueueFree();
        }
        Vector2 newPos = Position.MoveToward(TargetPos, (float)MovementVelocity * (float)delta);
        Position = newPos;
    }

    public new void BodyEntered(Node2D node)
    {
        if (node is Enemy)
        {
        }
    }
}
