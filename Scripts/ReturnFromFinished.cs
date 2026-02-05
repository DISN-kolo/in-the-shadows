using Godot;
using System;

public partial class ReturnFromFinished : Button
{
	private Signals NodeOfSignals;

	private void OnPressedYeah()
	{
		NodeOfSignals.EmitSignal(Signals.SignalName.ReturnFromFinished);
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		Pressed += OnPressedYeah;
	}

	public override void _ExitTree()
	{
		Pressed -= OnPressedYeah;
	}
}
