using Godot;
using System;

public partial class Selector : VBoxContainer
{
	private Signals NodeOfSignals;

	[Export]
	public int LevelNumber = 0;

	[Export]
	public string DesiredLabel = "";

	private void OnPressed()
	{
		// TODO conditional for the unlocks in the normal mode
		NodeOfSignals.EmitSignal(Signals.SignalName.PrepareLevel, LevelNumber);
	}

	public override void _Ready()
	{
		GetNode<Button>("SelectorButton").Pressed += OnPressed;
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		GetNode<Label>("SelectorLabel").Text = DesiredLabel;
	}

	public override void _ExitTree()
	{
		GetNode<Button>("SelectorButton").Pressed -= OnPressed;
	}
}
