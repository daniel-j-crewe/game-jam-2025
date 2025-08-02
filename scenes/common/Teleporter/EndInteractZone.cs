using Godot;
using System;

public partial class EndInteractZone : Area2D
{
    [Export]
    public TeleporterEndNode AttachedEndNode { get; set; }
    public new void BodyEntered(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            (node as cool_wizard_001).CurrentTeleporterEndNode = AttachedEndNode;
        }
    }

    public new void BodyExited(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            (node as cool_wizard_001).CurrentTeleporterEndNode = null;
        }
    }
}
