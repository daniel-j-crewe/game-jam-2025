using Godot;
using System;

public partial class DialogueTrigger : Area2D
{
    [Export]
    public string TextToShow { get; set; }
    private bool hasTriggered = false;
    public new void BodyEntered(Node2D node)
    {
        if (node is cool_wizard_001 && !hasTriggered)
        {
            (node as cool_wizard_001).SetPlayerText(TextToShow);
            hasTriggered = true;
        }
    }
}
