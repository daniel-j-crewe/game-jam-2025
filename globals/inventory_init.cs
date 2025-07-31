using Godot;

public partial class PlayerVariables : Node
{
    private static int _turretCount = 0;

    public static int TurretCount => _turretCount;

    public static void IncreaseTurretCount(int amount = 1)
    {
        _turretCount += amount;
    }

    public static void DecreaseTurretCount(int amount = 1)
    {
        if (_turretCount == 0)
        {
            GD.PushError("PlayerVariables: cannot decrease turret count below zero.");
            return;
        }

        _turretCount -= amount;
    }


}
