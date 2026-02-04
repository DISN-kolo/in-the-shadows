using Godot;

public partial class DebugSignals : Node
{
	public static DebugSignals Instance { get; private set; }

	public static Vector3 OffsetCurrent { get; set; }
	
	[Signal]
	public delegate void FirstSpawnedEventHandler(CharacterBody3D SC);

	public override void _Ready()
	{
		Instance = this;
		OffsetCurrent = new Vector3(0, 0, 0);
	}
}
