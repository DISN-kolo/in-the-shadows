using Godot;
using System;

public partial class ShadowCaster : CharacterBody3D
{
	private DebugSignals NodeOfDebugSignals;

	public bool Activated = false;
	public bool HMovOnly = false;
	public int Number = 0;
	public bool LMBDown = false;
	public Vector2 Target = new Vector2(0, 0);
	public Vector2 MovTarget = new Vector2(0, 0);
	public Vector3 TargetReal = new Vector3(0, 0, 0);
	public Vector3 MovTargetReal = new Vector3(0, 0, 0);
	public Vector2 ScreenSize = new Vector2(0, 0);

	public Vector3 IntendedRot = new Vector3(0, 0, 0);
//	public Vector3 IntendedPos = new Vector3(0, 0, 0);

	public string MeshScenePath { get; set; } = "";
	public PackedScene TempVarForMeshScene;
	public Node InstanceOfTempMeshScene;

	public float Epsilon = 0.1f;

	public bool CurrentlyInsideSolution = false;

	public bool FlippableX = false;
	public bool FlippableY = false;

	public bool MoveMode = false;

	public float LocalDepth = 0.0f;

	public bool SolutionFinalized = false;

	[Signal]
	public delegate void ImInRotationEventHandler(int MyNumber);

	[Signal]
	public delegate void ImOuttaRotationEventHandler(int MyNumber);


	private float SelectedXRotFinal = 0.0f;
	private float SelectedYRotFinal = 0.0f;

	public void SetRotTgtToClosestTgt()
	{
		TargetReal = new Vector3(SelectedXRotFinal, SelectedYRotFinal, 0.0f);
	}

	private void SetFinalRot(bool XOrY, float Tgt)
	{
		if (XOrY)
		{
			SelectedXRotFinal = Tgt;
		}
		else
		{
			SelectedYRotFinal = Tgt;
		}
	}

	private bool AreAnglesClose(float Rot, float Tgt, float Diff, bool XOrY)
	{
		bool localFlip = false;
		if (XOrY)
		{
			localFlip = FlippableX;
		}
		else
		{
			localFlip = FlippableY;
		}
		float TgtAdj = Tgt + 2*(float)Math.PI;
		if ((Rot + Diff >= Tgt) && (Rot - Diff <= Tgt))
		{
			SetFinalRot(XOrY, Tgt);
			GD.Print("setting to Tgt");
			return true;
		}
		else if ((Rot + Diff >= TgtAdj) && (Rot - Diff <= TgtAdj))
		{
			SetFinalRot(XOrY, TgtAdj);
			GD.Print("setting to TgtAdj");
			return true;
		}
		if (localFlip)
		{
			float TgtFlipped = Tgt + (float)Math.PI;
			float TgtFlippedAdj = TgtAdj + (float)Math.PI;
			if ((Rot + Diff >= TgtFlipped) && (Rot - Diff <= TgtFlipped))
			{
				SetFinalRot(XOrY, TgtFlipped);
				GD.Print("setting to TgtFlipped");
				return true;
			}
			if ((Rot + Diff >= TgtFlippedAdj) && (Rot - Diff <= TgtFlippedAdj))
			{
				SetFinalRot(XOrY, TgtFlippedAdj);
				GD.Print("setting to TgtFlippedAdj");
				return true;
			}
		}
		return false;
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

	private void RotationRandomizer(Random rand)
	{
		if (HMovOnly)
		{
			Rotation = Rotation with {
				X = IntendedRot.X,
				Y = IntendedRot.Y + ((float)rand.NextDouble()/2.0f + 0.25f) * (float)Math.PI
			};
		}
		else
		{
			Rotation = Rotation with {
				X = (float)rand.NextDouble() * (float)Math.PI * 2.0f,
				Y = (float)rand.NextDouble() * (float)Math.PI * 2.0f
			};
		}
	}

	public override void _Ready()
	{
		ScreenSize = GetViewport().GetVisibleRect().Size;
		var rand = new Random();
		RotationRandomizer(rand);
		while (AreRotsClose(Rotation, IntendedRot, Epsilon))
		{
			RotationRandomizer(rand);
		}

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
		NodeOfDebugSignals = GetNode<DebugSignals>("/root/DebugSignals");
		GD.Print("I really need to emit 'first spawned'");
		NodeOfDebugSignals.EmitSignal(DebugSignals.SignalName.FirstSpawned, this);
		Signals.Instance.AskUpdateMoveMode += OnAskedUpdateMoveMode;
		Signals.Instance.ActivateObject += OnActivatedObject;
		MoveMode = Signals.CurrentMoveMode;
	}

	public override void _Process(double delta)
	{
		if (!SolutionFinalized)
		{
			MovTargetReal = CalculateMov();
			TargetReal = CalculateAngle();
		}
		Rotation = Rotation with {
			X = (float)Mathf.Lerp(
				Rotation.X, TargetReal.X, delta * Settings.Instance.RotateVel
			),
			Y = (float)Mathf.Lerp(
				Rotation.Y, TargetReal.Y, delta * Settings.Instance.RotateVel
			)
		};
		Position = Position with {
			X = (float)Math.Clamp(Mathf.Lerp(
				Position.X, MovTargetReal.X, delta * Settings.Instance.RotateVel
			), -3.0, 3.0),
			Y = (float)Math.Clamp(Mathf.Lerp(
				Position.Y, MovTargetReal.Y, delta * Settings.Instance.RotateVel
			), -3.0, 3.0)
		};

		if (!SolutionFinalized)
		{
			if (AreRotsClose(Rotation, IntendedRot, Epsilon))
			{
				if (!LMBDown && !CurrentlyInsideSolution)
				{
					GD.Print("In solution margins for SC number ", Number);
					CurrentlyInsideSolution = true;
					EmitSignal(SignalName.ImInRotation, Number);
				}
			}
			else
			{
				if (CurrentlyInsideSolution)
				{
					GD.Print("Left solution margins for SC number ", Number);
					CurrentlyInsideSolution = false;
					EmitSignal(SignalName.ImOuttaRotation, Number);
				}
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!Activated || SolutionFinalized)
		{
			return ;
		}
		if (Input.IsActionJustPressed("LMB"))
		{
			LMBDown = true;
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
					if (HMovOnly)
					{
						Target += EventMouseMotion.Relative
							* new Vector2(
								(float)Settings.Instance.MouseSens,
								0.0f);
					}
					else
					{
						Target += EventMouseMotion.Relative
							* new Vector2(
								(float)Settings.Instance.MouseSens,
								(float)Settings.Instance.MouseSens
								);
					}
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

	public override void _ExitTree()
	{
		Signals.Instance.AskUpdateMoveMode -= OnAskedUpdateMoveMode;
		Signals.Instance.ActivateObject -= OnActivatedObject;
	}
}
