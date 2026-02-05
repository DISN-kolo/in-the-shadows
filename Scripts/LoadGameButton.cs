using Godot;
using System;

public partial class LoadGameButton : Button
{
	private Signals NodeOfSignals;

	private void OnPressed()
	{
		NodeOfSignals.EmitSignal(Signals.SignalName.LoadGame);
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
