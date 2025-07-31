using Godot;

public partial class InventoryManager : Node
{
	private Label _turretCountLabel;

	public override void _Ready()
	{
		_turretCountLabel = GetNode<Label>("Label");

		UpdateTurretLabel();

	}

	public override void _Process(double delta)
	{
		UpdateTurretLabel();
	}

	private void UpdateTurretLabel()
	{
		_turretCountLabel.Text = $"Turrets: {PlayerInventory.TurretCount}";
	}
}
