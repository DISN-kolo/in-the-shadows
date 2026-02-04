using Godot;
using System;

public partial class GetOutButton : Button
{
	private void OnPressed()
	{
		GetTree().Quit();
	}

	public override void _Ready()
	{
		Pressed += OnPressed;
	}

	public override void _ExitTree()
	{
		Pressed -= OnPressed;
	}
}
