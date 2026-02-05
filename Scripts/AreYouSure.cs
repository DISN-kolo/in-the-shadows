using Godot;
using System;

public partial class AreYouSure : Control
{
	private Signals NodeOfSignals;

	private void OnPressedNope()
	{
		QueueFree();
		NodeOfSignals.EmitSignal(Signals.SignalName.SayNoToBacking);
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		GetNode<Button>("PanelContainer/VBoxContainer/HBoxContainer/BackNopeButton").Pressed += OnPressedNope;
	}

	public override void _ExitTree()
	{
		GetNode<Button>("PanelContainer/VBoxContainer/HBoxContainer/BackNopeButton").Pressed -= OnPressedNope;
	}
}
