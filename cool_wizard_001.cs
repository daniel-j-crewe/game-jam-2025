using Godot;
using System;

public partial class cool_wizard_001 : CharacterBody2D
{
    Vector2 velocity;
    [Export]
    public float Speed { get; set; }
    [Export]
    public AnimatedSprite2D AnimatedSprite2D { get; set; }
    public override void _Ready()
    {
        base._Ready();
        AnimatedSprite2D.Play();
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
        velocity.X = direction.X * Speed;
        velocity.Y = direction.Y * Speed;
        Velocity = velocity;
        MoveAndSlide();
    }

}
