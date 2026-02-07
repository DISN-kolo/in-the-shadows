using Godot;
using System;

public partial class RightBordering : TextureRect
{
	private double MPosX;
	private double VPSizeX;
	public double Intensity = 0.0;
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
			Intensity = Math.Clamp(1.0f + (MPosX - VPSizeX)/Size.X, 0.0f, 1.0f);
			ThingMat.SetShaderParameter("intensity", Intensity);
		}
		else
		{
			Intensity = 0.0;
			ThingMat.SetShaderParameter("intensity", 0.0);
		}
	}
}
