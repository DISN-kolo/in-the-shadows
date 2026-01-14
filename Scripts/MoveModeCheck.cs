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
		this.Toggled += OnToggledMoveMode;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
