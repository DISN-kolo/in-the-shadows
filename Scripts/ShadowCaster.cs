using Godot;
using System;

public partial class ShadowCaster : CharacterBody3D
{
	public bool LMBDown = false;
	public Vector2 Target = new Vector2(0, 0);
	public Vector2 ScreenSize = new Vector2(0, 0);

	private Vector2 CalculateAngle()
	{
		Vector2 res = new Vector2((float)Target.X / (float)ScreenSize.X * (float)Math.PI * (float)Settings.MouseSens,
				(float)Target.Y / (float)ScreenSize.Y * (float)Math.PI * (float)Settings.MouseSens);
		return res;
	}

	private double CalculateAngleX()
	{
		double res = 0;
		res = Target.X / ScreenSize.X * Math.PI * Settings.MouseSens;
		return res;
	}

	public override void _Ready()
	{
		ScreenSize = GetViewport().GetVisibleRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Rotation = Rotation with {
			Y = (float)Mathf.Lerp(Rotation.Y, CalculateAngle().X, delta * Settings.RotateVel),
			X = (float)Mathf.Lerp(Rotation.X, CalculateAngle().Y, delta * Settings.RotateVel)};
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
