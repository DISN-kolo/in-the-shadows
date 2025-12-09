using Godot;
using System;

public partial class ShadowCaster : CharacterBody3D
{
	public bool LMBDown = false;
	public Vector2 Target = new Vector2(0, 0);
	public Vector3 TargetRot = new Vector3(0, 0, 0);
	public Vector2 ScreenSize = new Vector2(0, 0);

	public string MeshScenePath { get; set; } = "";
	public PackedScene TempVarForMeshScene;
	public Node InstanceOfTempMeshScene;

	private Vector3 CalculateAngle()
	{
		Vector3 res = new Vector3(
			Target.Y / ScreenSize.Y * (float)Math.PI,
			Target.X / ScreenSize.X * (float)Math.PI,
			0.0f
		);
		return res;
	}

	public override void _Ready()
	{
		ScreenSize = GetViewport().GetVisibleRect().Size;
		var rand = new Random();
		Rotation = Rotation with {
			X = (float)rand.NextDouble() * (float)Math.PI * 2.0f,
			Y = (float)rand.NextDouble() * (float)Math.PI * 2.0f
		};
		TargetRot = Rotation;
		Target = Target with {
			X = Rotation.Y * ScreenSize.X / (float)Math.PI,
			Y = Rotation.X * ScreenSize.Y / (float)Math.PI
		};
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
		TargetRot = CalculateAngle();
//		GD.Print("pro rot: ", Rotation);
//		GD.Print("pro tgr: ", TargetRot);
//		GD.Print("pro tgt: ", Target);
		Rotation = Rotation with {
			X = (float)Mathf.Lerp(
				Rotation.X, TargetRot.X, delta * Settings.Instance.RotateVel
			),
			Y = (float)Mathf.Lerp(
				Rotation.Y, TargetRot.Y, delta * Settings.Instance.RotateVel
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
				Target += eventMouseMotion.Relative * new Vector2((float)Settings.Instance.MouseSens, (float)Settings.Instance.MouseSens);
			}
		}
	}
}
