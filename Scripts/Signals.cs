using Godot;

public partial class Signals : Node
{
	public static Signals Instance { get; private set; }

	[Signal]
	public delegate void LevelsInitializedEventHandler();

	[Signal]
	public delegate void AskToChangeLevelEventHandler(int NextLevel);

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
