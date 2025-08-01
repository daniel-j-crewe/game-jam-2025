using Godot;
using System;

public partial class TurretPlacementIndicator : AnimatedSprite2D
{
    [Export]
    public AnimatedSprite2D animatedSprite2D { get; set; }
    public bool isCurrentlyValid { get; set; }
    public override void _Ready()
    {
        base._Ready();
        animatedSprite2D.Play();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        (this.Material as ShaderMaterial).SetShaderParameter("isValid", isCurrentlyValid);
    }

}
