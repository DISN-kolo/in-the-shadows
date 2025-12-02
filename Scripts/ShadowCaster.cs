using Godot;
using System;

public partial class ShadowCaster : CharacterBody3D
{
	public double seconds = 0.0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("LMB")) {
			GD.Print("You've been held down for... ", seconds, " seconds");
			seconds += delta;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			GD.Print("Mouse Motion at: ", eventMouseMotion.Position);
			GD.Print("Event relative: ", eventMouseMotion.Relative);
		}
//		GD.Print("Viewport Resolution is: ", GetViewport().GetVisibleRect().Size);
	}
}
