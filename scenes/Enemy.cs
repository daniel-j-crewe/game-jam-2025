using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class Enemy : CharacterBody2D
{
    [ExportAttribute]
    public Vector2 StartPos;
    [ExportAttribute]
    public float Velocity;
    public Vector2[] TargetPosArray;
    [ExportAttribute]
    public EnemyWaypoint[] Waypoints;
    private List<Vector2> TargetPositions = new List<Vector2>();
    private Vector2 CurrentTargetPos;
    private int CurrentTargetIndex = 0;
    [Export]
    public NavigationAgent2D NavAgent { get; set; }
    [Export]
    public cool_wizard_001 player { get; set; }
    [Export]
    public bool AggroTrackingEnabled { get; set; }
    double navUpdateTimer = 0;
    double navUpdateTime = 0.2;

    public override void _Ready()
    {
        base._Ready();
        GlobalPosition = StartPos;
        TargetPositions = Waypoints.Select(x => x.GlobalPosition).ToList();
        OrganiseTargetPositions();
        CurrentTargetPos = TargetPositions[0];
        NavAgent.TargetPosition = player.GlobalPosition;

    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        navUpdateTimer -= delta;
        if (navUpdateTimer < 0)
        {
            navUpdateTimer = navUpdateTime;
            NavAgent.TargetPosition = player.GlobalPosition;
        }
        Vector2 newPos;
        if (AggroTrackingEnabled)
        {
            newPos = GlobalPosition.MoveToward(NavAgent.GetNextPathPosition(), Velocity * (float)delta);
        }
        else
        {
            newPos = GlobalPosition.MoveToward(CurrentTargetPos, Velocity * (float)delta);
        }
        if (Position == CurrentTargetPos)
        {
            CurrentTargetPos = GetNextTargetPos();
        }
        GlobalPosition = newPos;
    }


    public void VisionZoneEntered()
    {
        AggroTrackingEnabled = true;
    }


    private void OrganiseTargetPositions()
    {
        if (TargetPositions.Count == 0)
        {
            TargetPositions.Add(StartPos);
        }
        else
        {
            TargetPositions.Reverse();
            List<Vector2> returnPath = new List<Vector2>();
            returnPath.AddRange(TargetPositions);
            TargetPositions.Reverse();
            TargetPositions.AddRange(returnPath);
            TargetPositions.Add(StartPos);
        }
    }

    private Vector2 GetNextTargetPos()
    {
        CurrentTargetIndex++;
        if (CurrentTargetIndex > TargetPositions.Count - 1)
        {
            CurrentTargetIndex = 0;
        }
        return TargetPositions[CurrentTargetIndex];
    }
}
