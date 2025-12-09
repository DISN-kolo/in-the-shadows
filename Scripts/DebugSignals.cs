using Godot;

public partial class DebugSignals : Node
{
	public static DebugSignals Instance { get; private set; }
	
	[Signal]
	public delegate void FirstSpawnedEventHandler(CharacterBody3D SC);
	
	public override void _Ready()
	{
		Instance = this;
	}
}
