using Godot;
using System;

public partial class TextBubble : Control
{
    [Export]
    public Label TextLabel { get; set; }
    [Export]
    public double TimeToShowS { get; set; }
    private double timer;


    public void SetText(string text)
    {
        TextLabel.Text = text;
        timer = TimeToShowS;
        this.Visible = true;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        timer -= delta;
        if (timer < 0)
        {
            this.Visible = false;
        }
    }

}
