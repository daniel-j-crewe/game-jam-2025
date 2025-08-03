using Godot;
using System;

public partial class Penguin : AnimatedSprite2D
{
    [Export]
    public AnimatedSprite2D sprite { get; set; }

    public override void _Ready()
    {
        base._Ready();
        sprite.Play();
    }

}
