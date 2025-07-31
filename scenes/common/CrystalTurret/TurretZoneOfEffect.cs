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
    }

    public new void BodyExited(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            AttachedTurret.PlayerLeftActivationZone();
        }
    }
}
