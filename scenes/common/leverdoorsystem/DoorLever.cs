using Godot;
using System;

public partial class DoorLever : Area2D
{
    [Export]
    public Door AttachedDoor { get; set; }
    public new void BodyEntered(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            (node as cool_wizard_001).CurrentDoorLever = this;
        }
    }

    public new void BodyExited(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            (node as cool_wizard_001).CurrentDoorLever = null;
        }
    }

    public void PullLever()
    {
        AttachedDoor.FlipState();
    }
}
