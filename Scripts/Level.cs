using Godot;
using System;

public partial class Level : Node3D
{
	private Signals NodeOfSignals;
	public int LevelNumber;

	private Vector3 OffsetOne = new Vector3(0.0f, 0.0f, 0.0f);
	private Vector3 OffsetTwo = new Vector3(0.0f, 0.0f, 0.0f);

	[Export]
	public string[] MeshScenesPaths = {};

	// please note the linear array. it's actually {x, y, z, x, y, z...}
	// please note the mathematical "type" of data entered. It should be radians divided by Pi.
	[Export]
	public float[] SCRotations = {};

	// please note the linear array. it's actually {x, y, z, x, y, z...}
	// please also note that the amount of offsets is equal to the amount
	//of meshes - 1.
	[Export]
	public float[] SCOffsets = {};

	[Export]
	public Godot.Collections.Array<bool> AllowFlippedVEx;

	[Export]
	public Godot.Collections.Array<bool> AllowFlippedHEx;

	public int MeshScenesAmt = 0;

	public PackedScene ShadowCasterScene;
	public ShadowCaster CurrentShadowCasterInstance;

	private bool[] InRotations = {};

	[Export]
	private float[] RotMargins = {};

	[Export]
	private double MoveMargin = 0.1;
	private bool InMovementSolution = false;

	private void CheckAllIns()
	{
		for (int i = 0; i < MeshScenesAmt; i++)
		{
			if (!InRotations[i])
			{
				GD.Print("Whoops! ", i, " isn't in rotation");
				return ;
			}
		}
		if (MeshScenesAmt == 2)
		{
			UnsolveMovement();
			GD.Print("Congratulations! Everyone is in correct rotation. Begin pos check");
			if (OffsetClose(OffsetOne, new Vector3(SCOffsets[0], SCOffsets[1], SCOffsets[2]), MoveMargin))
			{
				GD.Print("You're within marings");
				MovementSolved();
				return ;
			}
		}
		else if (MeshScenesAmt == 3)
		{
			UnsolveMovement();
			GD.Print("Congratulations! Everyone (3) is in correct rotation. Begin pos check");
			if (OffsetClose(OffsetOne, new Vector3(SCOffsets[0], SCOffsets[1], SCOffsets[2]), MoveMargin)
				&& OffsetClose(OffsetTwo, new Vector3(SCOffsets[3], SCOffsets[4], SCOffsets[5]), MoveMargin))
			{
				GD.Print("You're within triple marings");
				MovementSolved();
				return ;
			}
		}
		else
		{
			// XXX if you're ever gonna go for 4 meshes, remember to implement conditions here
			// or just refactor this to be n-mesh-ready but it ain't gonna get to this point prolly
			MovementSolved();
		}
	}

	private void OnSCInRot(int Number)
	{
		InRotations[Number] = true;
		CheckAllIns();
	}

	private void OnSCOuttaRot(int Number)
	{
		InRotations[Number] = false;
	}

	public void LoadLevel()
	{
		GD.Print("Loading level!, ", LevelNumber);
		InRotations = new bool[MeshScenesAmt];
		for (int i = 0; i < MeshScenesAmt; i++)
		{
			CurrentShadowCasterInstance = (ShadowCaster)ShadowCasterScene.Instantiate();
			if (i == 0)
			{
				CurrentShadowCasterInstance.Activated = true;
			}
			CurrentShadowCasterInstance.MeshScenePath = MeshScenesPaths[i];
			CurrentShadowCasterInstance.IntendedRot = new Vector3(
				SCRotations[i*3] * (float)Math.PI,
				SCRotations[i*3 + 1] * (float)Math.PI,
				SCRotations[i*3 + 2] * (float)Math.PI
			);
			CurrentShadowCasterInstance.FlippableX = AllowFlippedVEx[i];
			CurrentShadowCasterInstance.FlippableY = AllowFlippedHEx[i];
			CurrentShadowCasterInstance.Number = i;
			CurrentShadowCasterInstance.ImInRotation += OnSCInRot;
			CurrentShadowCasterInstance.ImOuttaRotation += OnSCOuttaRot;
			CurrentShadowCasterInstance.Epsilon = RotMargins[i];
			if (MeshScenesAmt > 1)
			{
				CurrentShadowCasterInstance.LocalDepth = -2.5f + i*2.5f;
			}
			if (LevelNumber == 0)
			{
				CurrentShadowCasterInstance.HMovOnly = true;
			}
			else
			{
				CurrentShadowCasterInstance.HMovOnly = false;
			}
			AddChild(CurrentShadowCasterInstance);
		}
		Visible = true;
		NodeOfSignals.EmitSignal(Signals.SignalName.LevelLoaded, MeshScenesAmt);
	}

	public void UnloadLevel()
	{
		foreach (ShadowCaster LocalNode in GetChildren())
		{
			LocalNode.QueueFree();
			LocalNode.ImInRotation -= OnSCInRot;
			LocalNode.ImOuttaRotation -= OnSCOuttaRot;
		}
		Visible = false;
	}

	private void MovementSolved()
	{
		InMovementSolution = true;
		GD.Print("Movement solved!");
		NodeOfSignals.EmitSignal(Signals.SignalName.LevelFinished, LevelNumber);
	}

	public override void _Ready()
	{
		MeshScenesAmt = MeshScenesPaths.Length;
		LevelNumber = GetIndex();
		GD.Print("LevelNumber: ", LevelNumber);
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		ShadowCasterScene = GD.Load<PackedScene>("res://Scenes/ShadowCaster.tscn");
		if (RotMargins == null || RotMargins.Length == 0)
		{
			RotMargins = new float[MeshScenesAmt];
			Array.Fill(RotMargins, 0.1f);
		}
		if (!Visible)
			return ;
		LoadLevel();
	}

	private bool OffsetClose(Vector3 Off, Vector3 Tgt, double Margin)
	{
		if ((Off.X - Margin > Tgt.X) || (Off.X + Margin < Tgt.X))
			return false;
		if ((Off.Y - Margin > Tgt.Y) || (Off.Y + Margin < Tgt.Y))
			return false;
		return true;
	}

	private void UnsolveMovement()
	{
		InMovementSolution = false;
		GD.Print("Unsolved movement...");
	}

	public override void _Process(double delta)
	{
		if (Visible && MeshScenesAmt == 2)
		{
			OffsetOne = ((CharacterBody3D)GetChildren()[1]).GlobalPosition - ((CharacterBody3D)GetChildren()[0]).GlobalPosition;
			DebugSignals.OffsetCurrent = OffsetOne;
			if (InMovementSolution && !OffsetClose(OffsetOne, new Vector3(SCOffsets[0], SCOffsets[1], SCOffsets[2]), MoveMargin))
			{
				UnsolveMovement();
			}
		}
		else if (Visible && MeshScenesAmt == 3)
		{
			OffsetOne = ((CharacterBody3D)GetChildren()[1]).GlobalPosition - ((CharacterBody3D)GetChildren()[0]).GlobalPosition;
			OffsetTwo = ((CharacterBody3D)GetChildren()[2]).GlobalPosition - ((CharacterBody3D)GetChildren()[0]).GlobalPosition;
			DebugSignals.OffsetCurrent = OffsetTwo;
			if (InMovementSolution
					&& !OffsetClose(OffsetOne, new Vector3(SCOffsets[0], SCOffsets[1], SCOffsets[2]), MoveMargin)
					&& !OffsetClose(OffsetTwo, new Vector3(SCOffsets[3], SCOffsets[4], SCOffsets[5]), MoveMargin))
			{
				UnsolveMovement();
			}
		}
	}

	public override void _ExitTree()
	{
		GD.Print("Quitting level. Children: ", GetChildren());
		if (Visible)
			UnloadLevel();
	}
}
