using Godot;

public partial class Settings : Node
{
	public static Settings Instance { get; private set; }

	public double MouseSens { get; set; }

	public override void _Ready()
	{
		Instance = this;
	}
}
