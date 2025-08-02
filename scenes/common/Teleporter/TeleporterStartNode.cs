using Godot;
using System;

public partial class TeleporterStartNode : Node2D
{
    [Export]
    public CollisionShape2D CollisionShape2D { get; set; }
    [Export]
    public TeleporterEndNode LinkedEndNode { get; set; }
    [Export]
    public AnimatedSprite2D Sprite { get; set; }
    public bool IsActive { get; set; }
    public void SetActive()
    {
        IsActive = true;
        CollisionShape2D.Disabled = false;
        this.Visible = true;
        Sprite.Play();
    }
}
