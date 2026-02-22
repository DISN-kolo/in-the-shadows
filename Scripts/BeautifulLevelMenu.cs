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
	private double DesiredStart = 0.0;

	[Export]
	public float MoveSpeed = 10.0f;

	public override void _Ready()
	{
		LeftRef = GetNode<LeftBordering>("LeftBordering");
		RightRef = GetNode<RightBordering>("RightBordering");
		MoveMe = GetNode<Control>("AllTheLevels");
		ScreenMid = GetViewport().GetVisibleRect().Size.X / 2.0;

		foreach (Selector LocalNode in MoveMe.GetChildren())
		{
			if (LocalNode.Position.X < MinX)
			{
				MinX = LocalNode.Position.X;
			}
			if (LocalNode.Position.X > MaxX)
			{
				MaxX = LocalNode.Position.X;
			}
			if (LocalNode.LevelNumber == Settings.Instance.MaxAvailableLevel)
			{
				DesiredStart = LocalNode.Position.X;
			}
		}
		MaxX += ScreenMid/3.0;
		MinX += ScreenMid/3.0;

		if (Settings.Instance.DevMode == false)
		{
			MoveMe.Position = MoveMe.Position with { X = (float)ScreenMid - (float)DesiredStart};
		}
	}

	public override void _Process(double delta)
	{
		ScreenMid = GetViewport().GetVisibleRect().Size.X / 2.0;
		float moveDelta = (float)delta * MoveSpeed * (-(float)RightRef.Intensity + (float)LeftRef.Intensity);
		if (MoveMe.Position.X >= ScreenMid - MinX)
		{
			MoveMe.Position = MoveMe.Position with { X = (float)ScreenMid - (float)MinX };
			if (RightRef.Intensity > 0.0)
			{
				MoveMe.Position = MoveMe.Position with { X = MoveMe.Position.X + moveDelta };
			}
		}
		else if (MoveMe.Position.X <= ScreenMid - MaxX)
		{
			MoveMe.Position = MoveMe.Position with { X = (float)ScreenMid - (float)MaxX };
			if (LeftRef.Intensity > 0.0)
			{
				MoveMe.Position = MoveMe.Position with { X = MoveMe.Position.X + moveDelta };
			}
		}
		else
		{
			MoveMe.Position = MoveMe.Position with { X = MoveMe.Position.X + moveDelta };
		}
	}
}
