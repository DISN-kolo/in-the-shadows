using Godot;
using System;

public partial class LeftBordering : TextureRect
{
	public double MPosX;
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
			ThingMat.SetShaderParameter("intensity", Math.Clamp(1.0f - MPosX / Size.X, 0.0f, 1.0f));
		}
		else
		{
			ThingMat.SetShaderParameter("intensity", 0.0);
		}
	}
}
