using Godot;
using System;

public partial class LastLevelFinished : PanelContainer
{
	public void SetAppropriateAnim()
	{
		GetNode<AnimationPlayer>("LLFAnim").SetCurrentAnimation("FadeIn");
	}

	public override void _Ready()
	{
	}
}
