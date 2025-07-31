using Godot;
using System;

public partial class TurretZoneOfEffect : Area2D
{
    [Export]
    public CrystalTurret AttachedTurret { get; set; }
    public new void BodyEntered(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            AttachedTurret.PlayerEnteredActivationZone();
        }
        if (node is Enemy)
        {
            AttachedTurret.EnemyEnteredActivationZone((node as Enemy));
        }
    }

    public new void BodyExited(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            AttachedTurret.PlayerLeftActivationZone();
        }
        if (node is Enemy)
        {
            AttachedTurret.EnemyLeftActivationZone((node as Enemy));
        }
    }
}
