using Godot;

public partial class DetectionZone : Area2D
{
	[Export]
	public Vampire AttachedEnemy { get; set; }
	public new void BodyEntered(Node2D node)
	{
		if (node is cool_wizard_001)
		{
			AttachedEnemy.VisionZoneEntered();
		}
	}
}
