using Godot;
using System;

public partial class ShadowCaster : CharacterBody3D
{
	public bool Activated = false;
	public int Number = 0;
	public bool LMBDown = false;
	public Vector2 Target = new Vector2(0, 0);
	public Vector2 MovTarget = new Vector2(0, 0);
	public Vector3 TargetReal = new Vector3(0, 0, 0);
	public Vector3 MovTargetReal = new Vector3(0, 0, 0);
	public Vector2 ScreenSize = new Vector2(0, 0);

	public Vector3 IntendedRot = new Vector3(0, 0, 0);
	public Vector3 IntendedPos = new Vector3(0, 0, 0);

	public string MeshScenePath { get; set; } = "";
	public PackedScene TempVarForMeshScene;
	public Node InstanceOfTempMeshScene;

	private Timer DiscoveredCorrectTimerNode;

	public float Epsilon = 0.01f;
	public float Delta = 0.1f;

	public bool CurrentlyInsideSolution = false;

	public bool FlippableX = false;
	public bool FlippableY = false;

	public bool MoveMode = false;

	public float LocalDepth = 0.0f;

	[Signal]
	public delegate void ImInRotationEventHandler(int MyNumber);

	[Signal]
	public delegate void ImOuttaRotationEventHandler(int MyNumber);

	private bool AreAnglesClose(float Rot, float Tgt, float Diff, bool XOrY)
	{
		float Temp = Tgt;
		bool localFlip = false;
		if (XOrY)
		{
			localFlip = FlippableX;
		}
		else
		{
			localFlip = FlippableY;
		}
		if (Rot > Tgt)
		{
			Tgt = Rot;
			Rot = Temp;
		}
		float OtherEnd = 0;
		if (localFlip)
		{
			OtherEnd = (float)Math.PI - Diff;
		}
		else
		{
			OtherEnd = 2 * (float)Math.PI - Diff;
		}
		if ((Tgt - Rot > Diff) && (Tgt - Rot < OtherEnd))
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

	private Vector3 CalculateMov()
	{
		MovTarget = MovTarget with {
			X = (float)Math.Clamp(MovTarget.X, 0.0, ScreenSize.X),
			Y = (float)Math.Clamp(MovTarget.Y, 0.0, ScreenSize.Y)
		};
		Vector3 res = new Vector3(
			MovTarget.X / ScreenSize.X * 6.0f - 3.0f,
			MovTarget.Y / ScreenSize.Y * 6.0f - 3.0f,
			LocalDepth
		);
		return res;
	}

	private void _OnDCTTimeout()
	{
		GD.Print("KARAMBA! from ", this);
		CurrentlyInsideSolution = true;
		EmitSignal(SignalName.ImInRotation, Number);
	}

	private void AbortCount()
	{
		CurrentlyInsideSolution = false;
		EmitSignal(SignalName.ImOuttaRotation, Number);
		if (!DiscoveredCorrectTimerNode.IsStopped())
		{
			DiscoveredCorrectTimerNode.Stop();
			GD.Print("Pre-stopped timer");
		}
	}

	private void OnAskedUpdateMoveMode(bool ToggledOn)
	{
		MoveMode = ToggledOn;
	}

	private void OnActivatedObject(int N)
	{
		if (N == Number)
		{
			Activated = true;
		}
		else
		{
			Activated = false;
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
		TargetReal = Rotation;
		Target = Target with {
			X = Rotation.Y * ScreenSize.X / (float)Math.PI,
			Y = Rotation.X * ScreenSize.Y / (float)Math.PI
		};
		Position = Position with {
			X = (float)rand.NextDouble() * 6.0f - 3.0f,
			Y = (float)rand.NextDouble() * 6.0f - 3.0f,
			Z = LocalDepth
		};
		MovTargetReal = Position;
		// 6 by 6 square of real estate where we move stuff.
		// must convert to REAL SCREEN PIXELS for the mouse stuff
		// therefore, this happens:
		MovTarget = MovTarget with {
			X = (Position.X + 3.0f) * ScreenSize.X / 6.0f,
			Y = (Position.Y + 3.0f) * ScreenSize.Y / 6.0f
		};
		TempVarForMeshScene = GD.Load<PackedScene>(MeshScenePath);
		InstanceOfTempMeshScene = TempVarForMeshScene.Instantiate();
		AddChild(InstanceOfTempMeshScene);
		var NodeOfDebugSignals = GetNode<DebugSignals>("/root/DebugSignals");
		GD.Print("I really need to emit 'first spawned'");
		NodeOfDebugSignals.EmitSignal(DebugSignals.SignalName.FirstSpawned, this);
		Signals.Instance.AskUpdateMoveMode += OnAskedUpdateMoveMode;
		DiscoveredCorrectTimerNode = GetNode<Timer>("./DiscoveredCorrectTimer");
		DiscoveredCorrectTimerNode.Timeout += _OnDCTTimeout;
		Signals.Instance.ActivateObject += OnActivatedObject;
		MoveMode = Signals.CurrentMoveMode;
	}

	public override void _Process(double delta)
	{
		TargetReal = CalculateAngle();
		Rotation = Rotation with {
			X = (float)Mathf.Lerp(
				Rotation.X, TargetReal.X, delta * Settings.Instance.RotateVel
			),
			Y = (float)Mathf.Lerp(
				Rotation.Y, TargetReal.Y, delta * Settings.Instance.RotateVel
			)
		};
		MovTargetReal = CalculateMov();
		Position = Position with {
			X = (float)Math.Clamp(Mathf.Lerp(
				Position.X, MovTargetReal.X, delta * Settings.Instance.RotateVel
			), -3.0, 3.0),
			Y = (float)Math.Clamp(Mathf.Lerp(
				Position.Y, MovTargetReal.Y, delta * Settings.Instance.RotateVel
			), -3.0, 3.0)
		};
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

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!Activated)
		{
			return ;
		}
		if (Input.IsActionJustPressed("LMB"))
		{
			LMBDown = true;
			AbortCount();
		}
		else if (Input.IsActionJustReleased("LMB"))
		{
			LMBDown = false;
		}
		if (@event is InputEventMouseMotion EventMouseMotion)
		{
			if (LMBDown)
			{
				if (!MoveMode)
				{
					Target += EventMouseMotion.Relative
						* new Vector2(
							(float)Settings.Instance.MouseSens,
							(float)Settings.Instance.MouseSens
							);
				}
				else
				{
					MovTarget += EventMouseMotion.Relative
						* new Vector2(
							(float)Settings.Instance.MouseSensMov,
							-(float)Settings.Instance.MouseSensMov
							);
				}
			}
		}
	}
}
