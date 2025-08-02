using Godot;
using System;

public partial class EnemyHitRange : Area2D
{
    [Export]
    public Enemy AttachedEnemy { get; set; }
    public new void BodyEntered(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            AttachedEnemy.signalController.EmitSignal("ResetLoop");
        }
    }

    public new void AreaEntered(Area2D node)
    {
        if (node is CrystalBullet)
        {
            AttachedEnemy.HitAndRemove();
            node.QueueFree();
        }
    }
}
