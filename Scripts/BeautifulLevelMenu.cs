using Godot;
using System;

public partial class BeautifulLevelMenu : Control
{
	private LeftBordering LeftRef;
	private RightBordering RightRef;
	private Control MoveMe;
	private double ScreenMid;
	private double MinX = 65536.0;
	private double MaxX = 0.0;

	[Export]
	public float MoveSpeed = 10.0f;

	public override void _Ready()
	{
		LeftRef = GetNode<LeftBordering>("LeftBordering");
		RightRef = GetNode<RightBordering>("RightBordering");
		MoveMe = GetNode<Control>("AllTheLevels");

		foreach (Control LocalNode in MoveMe.GetChildren())
		{
			if (LocalNode.Position.X < MinX)
			{
				MinX = LocalNode.Position.X;
			}
			if (LocalNode.Position.X > MaxX)
			{
				MaxX = LocalNode.Position.X;
			}
		}
	}

	public override void _Process(double delta)
	{
		ScreenMid = GetViewport().GetVisibleRect().Size.X / 2.0;
		if (MoveMe.Position.X >= ScreenMid - MinX)
		{
			MoveMe.Position = MoveMe.Position with { X = (float)ScreenMid - (float)MinX };
			if (RightRef.Intensity > 0.0)
			{
				MoveMe.Position = MoveMe.Position with { X = MoveMe.Position.X + (float)delta * MoveSpeed * (-(float)RightRef.Intensity + (float)LeftRef.Intensity)};
			}
		}
		else if (MoveMe.Position.X <= ScreenMid - MaxX)
		{
			MoveMe.Position = MoveMe.Position with { X = (float)ScreenMid - (float)MaxX };
			if (LeftRef.Intensity > 0.0)
			{
				MoveMe.Position = MoveMe.Position with { X = MoveMe.Position.X + (float)delta * MoveSpeed * (-(float)RightRef.Intensity + (float)LeftRef.Intensity)};
			}
		}
		else
		{
			MoveMe.Position = MoveMe.Position with { X = MoveMe.Position.X + (float)delta * MoveSpeed * (-(float)RightRef.Intensity + (float)LeftRef.Intensity)};
		}
	}
}
