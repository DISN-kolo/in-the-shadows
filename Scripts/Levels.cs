using Godot;
using System;

public partial class Levels : Node3D
{
	public int CurrentLevel = 0;
	private bool FirstLoaded = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ChangeLevelTo(0);
		Settings.Instance.LevelCount = this.GetChildren().Count;
		var NodeOfSignals = GetNode<Signals>("/root/Signals");
		NodeOfSignals.EmitSignal(Signals.SignalName.LevelsInitialized);
		Signals.Instance.AskToChangeLevel += ChangeLevelTo;
	}

	public void ChangeLevelTo(int NextLevel)
	{
		if (FirstLoaded)
		{
			Level TempCurrent = (Level)this.GetChildren()[CurrentLevel];
			TempCurrent.UnloadLevel();
		}
		else
		{
			FirstLoaded = true;
		}
		Level TempNext = (Level)this.GetChildren()[NextLevel];
		TempNext.LoadLevel();
		CurrentLevel = NextLevel;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
