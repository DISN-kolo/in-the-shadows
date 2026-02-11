using Godot;

public partial class Settings : Node
{
	public static Settings Instance { get; private set; }

	public double MouseSens { get; set; }
	public double MouseSensMov { get; set; }
	public double RotateVel { get; set; }
	public double MoveVel { get; set; }
	public int LevelCount { get; set; }
	public int YouNeedThisLevel { get; set; }
	public string SavePath { get; set; }
	public int MaxAvailableLevel { get; set; }
	public bool DevMode { get; set; }
	public bool[] CompletionShown;
	public bool[] UnlockShown;
	public string[] LevelNames;

	public override void _Ready()
	{
		Instance = this;
		MouseSens = 2.5;
		MouseSensMov = 2.5;
		RotateVel = 15.0;
		MoveVel = 10.0;
		YouNeedThisLevel = 0;
		SavePath = "./savefile";
		MaxAvailableLevel = 0;
		DevMode = false;
		CompletionShown = [false, false, false, false, false, false];
		UnlockShown = [false, false, false, false, false, false];
		LevelNames = [
			"Baby Don't Hurt Me", /* a very simple heart that can only be rotated horizontally */
			"Trunk",
			"England",
			"80 Days",
			"THE Answer",
			"Be Happy" /* stupid smiley thing */
		];
	}
}
