using Godot;
using System;

public partial class EndingZoneTriggerZone : Area2D
{
    [Export]
    public EndingZone AttachedEndZone { get; set; }
    [Export]
    public AudioStreamPlayer2D VictorySound { get; set; }
    public new void BodyEntered(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            AttachedEndZone.StartEnding();
            VictorySound.Play();
        }
    }
}
