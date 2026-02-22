using Godot;
using System;

public partial class GeneralInterface : Control
{
	private PackedScene LevelFinishedPS;
	private PackedScene LastLevelFinishedPS;
	private LevelFinished LFInstance;
	private LastLevelFinished LLFInstance;

	private void OnLevelFinished(int WhichLevel)
	{
		if (WhichLevel + 1 == Settings.Instance.LevelCount)
		{
			LLFInstance = (LastLevelFinished)LastLevelFinishedPS.Instantiate();
			LLFInstance.SetAppropriateAnim();
			AddChild(LLFInstance);
		}
		else
		{
			LFInstance = (LevelFinished)LevelFinishedPS.Instantiate();
			LFInstance.CurrentLevel = WhichLevel;
			LFInstance.SetAppropriateAnim();
			AddChild(LFInstance);
		}
	}

	public override void _Ready()
	{
		LevelFinishedPS = GD.Load<PackedScene>("res://Scenes/LevelFinished.tscn");
		LastLevelFinishedPS = GD.Load<PackedScene>("res://Scenes/LastLevelFinished.tscn");
		Signals.Instance.LevelFinished += OnLevelFinished;
	}

	public override void _ExitTree()
	{
		Signals.Instance.LevelFinished -= OnLevelFinished;
	}
}
