using Godot;
using System;

public partial class AreYouOverwriting : Control
{
	private void OnPressedNope()
	{
		QueueFree();
	}

	public override void _Ready()
	{
		GetNode<Button>("PanelContainer/VBoxContainer/HBoxContainer/OvNopeButton").Pressed += OnPressedNope;
	}

	public override void _ExitTree()
	{
		GetNode<Button>("PanelContainer/VBoxContainer/HBoxContainer/OvNopeButton").Pressed -= OnPressedNope;
	}
}
