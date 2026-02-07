using Godot;
using System;

public partial class RightBordering : TextureRect
{
	public double MPosX;
	private double VPSizeX;
	private ShaderMaterial ThingMat;

	public override void _Ready()
	{
		ThingMat = (ShaderMaterial)GetMaterial();
	}

	public override void _Process(double delta)
	{
		VPSizeX = GetViewport().GetVisibleRect().Size.X;
		MPosX = GetViewport().GetMousePosition().X;
		if (MPosX > VPSizeX - Size.X)
		{
			ThingMat.SetShaderParameter("intensity", Math.Clamp(1.0f + (MPosX - VPSizeX)/Size.X, 0.0f, 1.0f));
		}
		else
		{
			ThingMat.SetShaderParameter("intensity", 0.0);
		}
	}
}
