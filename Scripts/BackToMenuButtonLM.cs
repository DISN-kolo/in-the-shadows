using Godot;
using System;

public partial class BackToMenuButtonLM : Button
{
	private Signals NodeOfSignals;

	private void OnPressedYeah()
	{
		NodeOfSignals.EmitSignal(Signals.SignalName.BackFromLevels);
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
