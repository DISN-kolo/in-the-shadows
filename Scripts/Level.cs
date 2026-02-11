using Godot;
using System;

public partial class Level : Node3D
{
	private Signals NodeOfSignals;
	public int LevelNumber;

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

	private double MagicMargin = 0.1;
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
			if (OffsetClose(DebugSignals.OffsetCurrent, new Vector3(SCOffsets[0], SCOffsets[1], SCOffsets[2]), MagicMargin))
			{
				GD.Print("You're within marings");
				MovementSolved();
				return ;
			}
		}
		else
		{
			// XXX if you're ever gonna go for 3 meshes, remember to implement better conditions here
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
		GD.Print("BAM!, ", LevelNumber);
		MeshScenesAmt = MeshScenesPaths.Length;
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
			if (MeshScenesAmt > 1)
			{
				CurrentShadowCasterInstance.LocalDepth = -2 + i*3;
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
		foreach (var LocalNode in GetChildren())
		{
			LocalNode.QueueFree();
			((ShadowCaster)LocalNode).ImInRotation -= OnSCInRot;
			((ShadowCaster)LocalNode).ImOuttaRotation -= OnSCOuttaRot;
		}
		Visible = false;
	}

	private void MovementSolved()
	{
		InMovementSolution = true;
		GD.Print("Movement solved!");
		NodeOfSignals.EmitSignal(Signals.SignalName.LevelFinished, LevelNumber);
		GD.Print("YOU NEED TO BLOCK CONTROLS IMMEDIATELY"); // TODO
	}

	public override void _Ready()
	{
		LevelNumber = GetIndex();
		GD.Print("LevelNumber: ", LevelNumber);
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		ShadowCasterScene = GD.Load<PackedScene>("res://Scenes/ShadowCaster.tscn");
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
			DebugSignals.OffsetCurrent = ((CharacterBody3D)GetChildren()[1]).GlobalPosition - ((CharacterBody3D)GetChildren()[0]).GlobalPosition;
			if (InMovementSolution && !OffsetClose(DebugSignals.OffsetCurrent, new Vector3(SCOffsets[0], SCOffsets[1], SCOffsets[2]), MagicMargin))
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
