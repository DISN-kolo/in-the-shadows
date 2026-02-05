using Godot;
using System;

public partial class GeneralInterface : Control
{
	private PackedScene LevelFinishedPS;
	private LevelFinished LFInstance;

	private void OnLevelFinished(int WhichLevel)
	{
		LFInstance = (LevelFinished)LevelFinishedPS.Instantiate();
		LFInstance.CurrentLevel = WhichLevel;
		AddChild(LFInstance);
		// here we prolly should only make the window show up
	}

	public override void _Ready()
	{
		LevelFinishedPS = GD.Load<PackedScene>("res://Scenes/LevelFinished.tscn");
		Signals.Instance.LevelFinished += OnLevelFinished;
	}

	public override void _ExitTree()
	{
		Signals.Instance.LevelFinished -= OnLevelFinished;
	}
}
