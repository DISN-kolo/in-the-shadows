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
		if ((LevelNumber > Settings.Instance.MaxAvailableLevel) && (Settings.Instance.DevMode == false))
		{
			GetNode<Button>("SelectorButton").Disabled = true;
			GetNode<Label>("SelectorLabel").Text = "???";
		}
		else
		{
			GetNode<Label>("SelectorLabel").Text = DesiredLabel;
		}
	}

	public override void _ExitTree()
	{
		GetNode<Button>("SelectorButton").Pressed -= OnPressed;
	}
}
