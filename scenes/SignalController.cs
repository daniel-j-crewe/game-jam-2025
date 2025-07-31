using Godot;
using System;

public partial class SignalController : Node2D
{
    [Signal]
    public delegate void ResetLoopEventHandler();
}
