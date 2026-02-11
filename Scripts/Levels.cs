using Godot;
using System;

public partial class Levels : Node3D
{
	public int CurrentLevel = 0;
	private Label LevelName;
	private bool FirstLoaded = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LevelName = GetNode<Label>("../GeneralInterface/LevelNameContainer/LevelNamePanel/LevelName");
		ChangeLevelTo(Settings.Instance.YouNeedThisLevel);
		Settings.Instance.LevelCount = GetChildren().Count;
		var NodeOfSignals = GetNode<Signals>("/root/Signals");
		NodeOfSignals.EmitSignal(Signals.SignalName.LevelsInitialized);
		Signals.Instance.AskToChangeLevel += ChangeLevelTo;
//		int x = 0;
//		foreach (Level LocalNode in GetChildren())
//		{
//			LocalNode.LevelNumber = x;
//			GD.Print(x);
//			GD.Print(LocalNode.LevelNumber);
//			x += 1;
//		}
	}

	public void ChangeLevelTo(int NextLevel)
	{
		if (FirstLoaded)
		{
			Level TempCurrent = (Level)GetChildren()[CurrentLevel];
			TempCurrent.UnloadLevel();
		}
		else
		{
			FirstLoaded = true;
		}
		Level TempNext = (Level)GetChildren()[NextLevel];
		TempNext.LoadLevel();
		CurrentLevel = NextLevel;
		LevelName.Text = Settings.Instance.LevelNames[CurrentLevel];
	}

	public override void _ExitTree()
	{
		Signals.Instance.AskToChangeLevel -= ChangeLevelTo;
	}
}
