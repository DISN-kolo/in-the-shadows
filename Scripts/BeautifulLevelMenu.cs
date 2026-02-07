using Godot;
using System;

public partial class BeautifulLevelMenu : Control
{
	private LeftBordering LeftRef;
	private RightBordering RightRef;
	private Control MoveMe;

	[Export]
	public float MoveSpeed = 10.0f;

	public override void _Ready()
	{
		LeftRef = GetNode<LeftBordering>("LeftBordering");
		RightRef = GetNode<RightBordering>("RightBordering");
		MoveMe = GetNode<Control>("AllTheLevels");
	}

	public override void _Process(double delta)
	{
		MoveMe.Position = MoveMe.Position with { X = MoveMe.Position.X + (float)delta * MoveSpeed * (-(float)RightRef.Intensity + (float)LeftRef.Intensity)};
	}
}
