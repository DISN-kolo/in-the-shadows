using Godot;
using System;

public partial class LeftBordering : TextureRect
{
	private double MPosX;
	public double Intensity = 0.0;
	private ShaderMaterial ThingMat;

	public override void _Ready()
	{
		ThingMat = (ShaderMaterial)GetMaterial();
	}

	public override void _Process(double delta)
	{
		MPosX = GetViewport().GetMousePosition().X;
		if (MPosX < Size.X)
		{
			Intensity = Math.Clamp(1.0f - MPosX / Size.X, 0.0f, 1.0f);
			ThingMat.SetShaderParameter("intensity", Intensity);
		}
		else
		{
			ThingMat.SetShaderParameter("intensity", 0.0);
			Intensity = 0.0;
		}
	}
}
