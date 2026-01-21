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

	[Signal]
	public delegate void ActivateObjectEventHandler(int Number);

	[Signal]
	public delegate void LevelLoadedEventHandler(int MeshesAmt);

	public override void _Ready()
	{
		Instance = this;
	}
}
