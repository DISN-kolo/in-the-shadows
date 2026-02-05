using Godot;
using System;

public partial class NextLevelPls : Button
{
	public int CurrentLevel = 0;
	private Signals NodeOfSignals;

	private void OnPressed()
	{
		NodeOfSignals.EmitSignal(Signals.SignalName.AskToChangeLevel, CurrentLevel + 1);
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
