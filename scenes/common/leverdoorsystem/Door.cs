using Godot;
using System;

public partial class Door : StaticBody2D
{
    [Export]
    public CollisionShape2D Hitbox { get; set; }
    public void Open()
    {
        GD.Print("Door opened yay");
        this.Hitbox.Disabled = true;
        this.Visible = false;
    }
}
