using Godot;
using System;

public partial class NewGameButton : Button
{
	private Signals NodeOfSignals;

	private void OnPressed()
	{
		NodeOfSignals.EmitSignal(Signals.SignalName.NewGame);
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		Pressed += OnPressed;
	}

	public override void _ExitTree()
	{
		Pressed -= OnPressed;
	}
}
