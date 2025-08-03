using Godot;
using System;

public partial class DoorLever : Area2D
{
    [Export]
    public Door AttachedDoor { get; set; }
    [Export]
    public AudioStreamPlayer2D DoorUnlockSound { get; set; }
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
        DoorUnlockSound.Play();
    }

    public override void _Ready()
    {
        base._Ready();

        AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        sprite.Play("default");

    }
}
