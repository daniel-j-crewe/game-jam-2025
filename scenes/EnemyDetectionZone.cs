using Godot;
using System;

public partial class EnemyDetectionZone : Area2D
{
    [Export]
    public Enemy AttachedEnemy { get; set; }
    public new void BodyEntered(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            AttachedEnemy.VisionZoneEntered();
        }
    }
}
