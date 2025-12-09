using Godot;
using System;

public partial class ShadowCaster : CharacterBody3D
{
	public bool LMBDown = false;
	public Vector2 Target = new Vector2(0, 0);
	public Vector2 ScreenSize = new Vector2(0, 0);

	public string MeshScenePath { get; set; } = "";
	public PackedScene TempVarForMeshScene;
	public Node InstanceOfTempMeshScene;

	private Vector2 CalculateAngle()
	{
		Vector2 res = new Vector2(
			(float)Target.X /
				(float)ScreenSize.X * (float)Math.PI * (float)Settings.Instance.MouseSens,
			(float)Target.Y /
				(float)ScreenSize.Y * (float)Math.PI * (float)Settings.Instance.MouseSens
		);
		return res;
	}

	private double CalculateAngleX()
	{
		double res = 0;
		res = Target.X / ScreenSize.X * Math.PI * Settings.Instance.MouseSens;
		return res;
	}

	public override void _Ready()
	{
		ScreenSize = GetViewport().GetVisibleRect().Size;
		TempVarForMeshScene = GD.Load<PackedScene>(MeshScenePath);
		InstanceOfTempMeshScene = TempVarForMeshScene.Instantiate();
		AddChild(InstanceOfTempMeshScene);
		var NodeOfDebugSignals = GetNode<DebugSignals>("/root/DebugSignals");
		GD.Print("boutta send");
		NodeOfDebugSignals.EmitSignal(DebugSignals.SignalName.FirstSpawned, this);
		GD.Print("sent");
	}

	public override void _Process(double delta)
	{
		Rotation = Rotation with {
			Y = (float)Mathf.Lerp(
				Rotation.Y, CalculateAngle().X, delta * Settings.Instance.RotateVel
			),
			X = (float)Mathf.Lerp(
				Rotation.X, CalculateAngle().Y, delta * Settings.Instance.RotateVel
			)
		};
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("LMB"))
		{
			LMBDown = true;
		}
		else if (Input.IsActionJustReleased("LMB"))
		{
			LMBDown = false;
		}
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			if (LMBDown)
			{
				Target += eventMouseMotion.Relative;
			}
		}
	}
}
