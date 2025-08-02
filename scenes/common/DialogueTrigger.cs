using Godot;
using System;

public partial class DialogueTrigger : Area2D
{
    [Export]
    public string TextToShow { get; set; }
    public new void BodyEntered(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            (node as cool_wizard_001).SetPlayerText(TextToShow);
        }
    }
}
