using Godot;

public partial class MunchZone : Area2D
{
	[Export] public Vampire AttachedEnemy { get; set; }
	[Export] public Area2D Area { get; set; }

	public void OnBodyEntered(Node2D node)
	{
		if (node is cool_wizard_001)
		{
			AttachedEnemy.MunchZoneToggle(true);
		}
	}

	public void OnBodyExited(Node2D node)
	{
		if (node is cool_wizard_001)
		{
			AttachedEnemy.MunchZoneToggle(false);
		}
	}

	public override void _Ready()
	{
		base._Ready();
		
		Area.BodyEntered += OnBodyEntered;
		Area.BodyExited += OnBodyExited;
		

	}
}
