using Godot;

public partial class BoneZone : Area2D
{
	[Export] public Enemy AttachedEnemy { get; set; }
	[Export] public Area2D Area { get; set; }

	public void OnBodyEntered(Node2D node)
	{
		if (node is cool_wizard_001)
		{
			AttachedEnemy.BoneZoneToggle(true);
		}
	}

	public void OnBodyExited(Node2D node)
	{
		if (node is cool_wizard_001)
		{
			AttachedEnemy.BoneZoneToggle(false);
		}
	}

	public override void _Ready()
	{
		base._Ready();
		
		Area.BodyEntered += OnBodyEntered;
		Area.BodyExited += OnBodyExited;
		

	}
}
