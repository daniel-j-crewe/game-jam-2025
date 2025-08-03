using Godot;
using System;
using System.Net.NetworkInformation;

public partial class EndingZone : Control
{
    [Export]
    public TextureRect EndScreen { get; set; }
    [Export]
    public TextureRect Credits { get; set; }
    [Export]
    public double TimeForEndScreen { get; set; }
    private double endTimer;
    private double creditTimer;
    bool isEndActive = false;

    public void StartEnding()
    {
        endTimer = TimeForEndScreen;
        isEndActive = true;
        EndScreen.Visible = true;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (isEndActive)
        {
            endTimer -= delta;
            if (endTimer < 0)
            {
                GD.Print("showcredit");

                isEndActive = false;
                EndScreen.Visible = false;
                Credits.Visible = true;
            }
        }
    }

}
