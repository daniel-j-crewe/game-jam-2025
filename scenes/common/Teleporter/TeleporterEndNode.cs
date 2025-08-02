using Godot;
using System;

public partial class TeleporterEndNode : Node2D
{
    [Export]
    public TeleporterStartNode LinkedStartNode { get; set; }
    [Export]
    public AnimatedSprite2D Sprite { get; set; }
    public bool IsActive { get; set; }
    public void SetActive()
    {
        IsActive = true;
        LinkedStartNode.SetActive();
        Sprite.Play();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        (Sprite.Material as ShaderMaterial).SetShaderParameter("isActive", IsActive);
    }


}
