using Godot;
using System;

public partial class StartInteractZone : Area2D
{
    [Export]
    public TeleporterStartNode AttachedStartNode { get; set; }
    public new void BodyEntered(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            (node as cool_wizard_001).CurrentTeleporterStartNode = AttachedStartNode;
        }
    }

    public new void BodyExited(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            (node as cool_wizard_001).CurrentTeleporterStartNode = null;
        }
    }
}
