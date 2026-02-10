using Godot;
using System;

public partial class MoveModeCheck : CheckButton
{
	private Signals NodeOfSignals;

	public void OnToggledMoveMode(bool ToggledOn)
	{
		NodeOfSignals.EmitSignal(Signals.SignalName.AskUpdateMoveMode, ToggledOn);
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		Toggled += OnToggledMoveMode;
		ButtonPressed = Signals.CurrentMoveMode;
	}

	public override void _ExitTree()
	{
		Toggled -= OnToggledMoveMode;
	}
}
