using Godot;

public partial class Settings : Node
{
	public static Settings Instance { get; private set; }

	public double MouseSens { get; set; }
	public double MouseSensMov { get; set; }
	public double RotateVel { get; set; }
	public double MoveVel { get; set; }
	public int LevelCount { get; set; }

	public override void _Ready()
	{
		Instance = this;
		MouseSens = 2.5;
		MouseSensMov = 2.5;
		RotateVel = 15.0;
		MoveVel = 10.0;
	}
}
