using Godot;
using System;

public partial class LevelFinished : PanelContainer
{
	public int CurrentLevel = 0;

	public void SetAppropriateAnim()
	{
		GetNode<AnimationPlayer>("LFAnim").SetCurrentAnimation("FadeIn");
	}

	private void OnInteracted()
	{
		QueueFree();
	}

	public override void _Ready()
	{
		GetNode<NextLevelPls>("CenterContainer/VBoxContainer/HBoxContainer/NextLevelPls").CurrentLevel = CurrentLevel;
		GetNode<NextLevelPls>("CenterContainer/VBoxContainer/HBoxContainer/NextLevelPls").Pressed += OnInteracted;
	}

	public override void _ExitTree()
	{
		GetNode<NextLevelPls>("CenterContainer/VBoxContainer/HBoxContainer/NextLevelPls").Pressed -= OnInteracted;
	}
}
