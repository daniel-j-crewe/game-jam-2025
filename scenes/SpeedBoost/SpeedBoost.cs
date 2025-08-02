using Godot;
using System;

public partial class SpeedBoost : Area2D
{
    [Export]
    public double BoostDuration { get; set; }
    private bool UsedThisCycle = false;
    public SignalController signalController;

    public override void _Ready()
    {
        base._Ready();
        signalController = GetNode<SignalController>("/root/MainSceneRoot/SignalController");
        signalController.ResetLoop += AcknowledgeResetLoop;
    }

    public new void BodyEntered(Node2D node)
    {
        if (node is cool_wizard_001)
        {
            if (!UsedThisCycle)
            {
                (node as cool_wizard_001).SetBoostTimer(BoostDuration);
                UsedThisCycle = true;
                this.Visible = false;
            }
        }
    }

    public void AcknowledgeResetLoop()
    {
        UsedThisCycle = false;
        this.Visible = true;
    }

}
