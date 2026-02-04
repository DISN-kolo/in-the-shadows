using Godot;

public partial class Signals : Node
{
	public static Signals Instance { get; private set; }

	[Signal]
	public delegate void LevelsInitializedEventHandler();

	// This signal should be used for changing levels when the Main3D is loaded.
	[Signal]
	public delegate void AskToChangeLevelEventHandler(int NextLevel);

	// This signal should be used to enter into a level from the level select scene.
	[Signal]
	public delegate void PrepareLevelEventHandler(int WhichLevel);

	[Signal]
	public delegate void AskUpdateMoveModeEventHandler(bool ToggledOn);
	public static bool CurrentMoveMode { get; set; }

	[Signal]
	public delegate void ActivateObjectEventHandler(int Number);

	[Signal]
	public delegate void LevelLoadedEventHandler(int MeshesAmt);

	[Signal]
	public delegate void SayNoToBackingEventHandler();

	[Signal]
	public delegate void SayYesToBackingEventHandler();

	[Signal]
	public delegate void NewGameEventHandler();

	[Signal]
	public delegate void NewDevGameEventHandler();

	private void OnAskUpdateMoveModeGlobal(bool ToggledOn)
	{
		CurrentMoveMode = ToggledOn;
	}

	public override void _Ready()
	{
		Instance = this;
		AskUpdateMoveMode += OnAskUpdateMoveModeGlobal;
		CurrentMoveMode = false;
	}
}
