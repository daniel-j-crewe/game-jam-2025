using Godot;

public partial class HitRange : Area2D
{
	[Export]
	public Vampire AttachedEnemy { get; set; }

	private int _hitsTaken = 0;

	public new void BodyEntered(Node2D node)
	{
		if (node is cool_wizard_001)
		{
			AttachedEnemy.signalController.EmitSignal("ResetLoop");
		}
	}

	public new void AreaEntered(Area2D node)
	{
		if (node is CrystalBullet)
		{

			node.QueueFree();

			_hitsTaken++;

			if (_hitsTaken == 2)
			{
				AttachedEnemy.HitAndRemove();
			}
		}
	}
}
