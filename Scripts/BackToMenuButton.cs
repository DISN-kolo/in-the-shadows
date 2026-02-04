using Godot;
using System;

public partial class BackToMenuButton : Button
{
	private PackedScene YesOrNoThing;
	private Control AYSInstance;

	private void OnPressedBackToMenu()
	{
		Disabled = true;
		AYSInstance = (Control)YesOrNoThing.Instantiate();
		AYSInstance.GlobalPosition = GetViewport().GetVisibleRect().Size;
		AYSInstance.GlobalPosition = AYSInstance.GlobalPosition with { X = AYSInstance.GlobalPosition.X / 2, Y = AYSInstance.GlobalPosition.Y / 2 };
		GetParent().AddChild(AYSInstance);
	}

	public override void _Ready()
	{
		YesOrNoThing = GD.Load<PackedScene>("res://Scenes/AreYouSure.tscn");
		Pressed += OnPressedBackToMenu;
	}
}
