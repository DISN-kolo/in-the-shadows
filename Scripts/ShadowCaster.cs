using Godot;
using System;

public partial class ShadowCaster : CharacterBody3D
{
	public bool LMBDown = false;
	public Vector2 Target = new Vector2(0, 0);
	public Vector3 TargetRot = new Vector3(0, 0, 0);
	public Vector2 ScreenSize = new Vector2(0, 0);

	public Vector3 IntendedRot = new Vector3(0, 0, 0);

	public string MeshScenePath { get; set; } = "";
	public PackedScene TempVarForMeshScene;
	public Node InstanceOfTempMeshScene;

	public int MyIndex { get; set; } = 0;

	private Timer DiscoveredCorrectTimerNode;

	public float Epsilon = 0.01f;
	public float Delta = 0.1f;

	public bool CurrentlyInsideSolution = false;

	public bool FlippableX = false;
	public bool FlippableY = false;

	private bool AreAnglesClose(float Rot, float Tgt, float Diff, bool XOrY)
	{
		float Temp = Tgt;
		if (Rot > Tgt)
		{
			Tgt = Rot;
			Rot = Temp;
		}
		float OtherEnd = 0;
		if (XOrY)
		{
			if (FlippableX)
			{
				OtherEnd = (float)Math.PI - Diff;
			}
			else
			{
				OtherEnd = 2 * (float)Math.PI - Diff;
			}
		}
		else
		{
			if (FlippableY)
			{
				OtherEnd = (float)Math.PI - Diff;
			}
			else
			{
				OtherEnd = 2 * (float)Math.PI - Diff;
			}
		}
		if ((Tgt - Rot > Diff) && (Tgt - Rot < (float)Math.PI - Diff))
		{
			return false;
		}
		return true;
	}

	private void VecPiRemainder(ref Vector3 Input, int Ax)
	{
		Input[Ax] = Input[Ax] % (float)Math.PI;
	}

	private void VecTwoPiRemainder(ref Vector3 Input, int Ax)
	{
		Input[Ax] = Input[Ax] % (2 * (float)Math.PI);
	}

	// Please keep in mind that all this flippability is here because we basically don't consider symmetry of 3d objects at all
	private bool AreRotsClose(Vector3 Rot, Vector3 Tgt, float Diff)
	{
		if (FlippableX)
		{
			VecPiRemainder(ref Rot, 0);
			VecPiRemainder(ref Tgt, 0);
		}
		else
		{
			VecTwoPiRemainder(ref Rot, 0);
			VecTwoPiRemainder(ref Tgt, 0);
		}
		if (FlippableY)
		{
			VecPiRemainder(ref Rot, 1);
			VecPiRemainder(ref Tgt, 1);
		}
		else
		{
			VecTwoPiRemainder(ref Rot, 1);
			VecTwoPiRemainder(ref Tgt, 1);
		}
		if (AreAnglesClose(Rot.X, Tgt.X, Diff, true)
			&& AreAnglesClose(Rot.Y, Tgt.Y, Diff, false))
		{
			return true;
		}
		return false;
	}

	private Vector3 CalculateAngle()
	{
		Vector3 res = new Vector3(
			Target.Y / ScreenSize.Y * (float)Math.PI,
			Target.X / ScreenSize.X * (float)Math.PI,
			0.0f
		);
		return res;
	}

	private void _OnDCTTimeout()
	{
		GD.Print("KARAMBA!");
		CurrentlyInsideSolution = true;
	}

	private void AbortCount()
	{
		CurrentlyInsideSolution = false;
		if (!DiscoveredCorrectTimerNode.IsStopped())
		{
			DiscoveredCorrectTimerNode.Stop();
			GD.Print("Pre-stopped timer");
		}
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
		NodeOfDebugSignals.EmitSignal(DebugSignals.SignalName.FirstSpawned, this);
		DiscoveredCorrectTimerNode = GetNode<Timer>("./DiscoveredCorrectTimer");
		DiscoveredCorrectTimerNode.Timeout += _OnDCTTimeout;
	}

	public override void _Process(double delta)
	{
		TargetRot = CalculateAngle();
		Rotation = Rotation with {
			X = (float)Mathf.Lerp(
				Rotation.X, TargetRot.X, delta * Settings.Instance.RotateVel
			),
			Y = (float)Mathf.Lerp(
				Rotation.Y, TargetRot.Y, delta * Settings.Instance.RotateVel
			)
		};
		// keep in mind this is yet to factor all the symmetry features. TODO
		if (AreRotsClose(Rotation, IntendedRot, Delta))
		{
			if (!LMBDown && DiscoveredCorrectTimerNode.IsStopped() && !CurrentlyInsideSolution)
			{
				DiscoveredCorrectTimerNode.Start();
				GD.Print("Started timer");
			}
		}
		else
		{
			AbortCount();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("LMB"))
		{
			LMBDown = true;
			AbortCount();
		}
		else if (Input.IsActionJustReleased("LMB"))
		{
			LMBDown = false;
		}
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			if (LMBDown)
			{
				Target += eventMouseMotion.Relative
					* new Vector2(
						(float)Settings.Instance.MouseSens,
						(float)Settings.Instance.MouseSens
						);
			}
		}
	}
}
