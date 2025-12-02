using Godot;
using System;

public partial class ShadowCaster : CharacterBody3D
{
	public bool LMBDown = false;
	public Vector2 Target = new Vector2(0, 0);
	public Vector2 ScreenSize = new Vector2(0, 0);

	private double CalculateAngle()
	{
		double res = 0;
		res = Target.X / ScreenSize.X * Math.PI;
		return res;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewport().GetVisibleRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Rotation = Rotation with { X = (float)Mathf.Lerp(Rotation.X, CalculateAngle(), delta * 10) };
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("LMB"))
		{
			GD.Print("action just pressed lmb during _input handling");
			LMBDown = true;
		}
		else if (Input.IsActionJustReleased("LMB"))
		{
			GD.Print("action just released lmb during _input handling");
			LMBDown = false;
		}
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			GD.Print("Mouse Motion at: ", eventMouseMotion.Position);
			GD.Print("Event relative: ", eventMouseMotion.Relative);
			if (LMBDown)
			{
				Target += eventMouseMotion.Relative;
				GD.Print("Target updated to: ", Target);
			}
		}
//		GD.Print("Viewport Resolution is: ", GetViewport().GetVisibleRect().Size);
	}
}
