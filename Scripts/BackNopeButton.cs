using Godot;
using System;

public partial class BackNopeButton : Button
{
	private Signals NodeOfSignals;

	private void OnPressedNope()
	{
		NodeOfSignals.EmitSignal(Signals.SignalName.SayNoToBacking);
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		Pressed += OnPressedNope;
	}
}
