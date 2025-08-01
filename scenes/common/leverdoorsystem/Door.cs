using Godot;
using System;

public partial class Door : StaticBody2D
{
    [Export]
    public CollisionShape2D Hitbox { get; set; }
    public void FlipState()
    {
        this.Hitbox.Disabled = !Hitbox.Disabled;
        this.Visible = !Visible;
    }

}
