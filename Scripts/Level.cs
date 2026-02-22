using Godot;
using System;

public partial class Level : Node3D
{
	private Signals NodeOfSignals;
	public int LevelNumber;

	private float[] RealOffsetArray = {};

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
	private float MoveMargin = 0.1f;
	private bool InMovementSolution = false;

	public float[] RotationCloseness = {};
	public float[] MoveCloseness = {};

	private bool OffsetClose(Vector2 Off, Vector2 Tgt, float Margin)
	{
		if ((Off.X - Margin > Tgt.X) || (Off.X + Margin < Tgt.X))
			return false;
		if ((Off.Y - Margin > Tgt.Y) || (Off.Y + Margin < Tgt.Y))
			return false;
		return true;
	}

	private bool AtLeastOneOffsetBad()
	{
		for (int i = 1; i < MeshScenesAmt; i++)
		{
			if (OffsetClose(
						new Vector2(
							RealOffsetArray[(i - 1)*2],
							RealOffsetArray[(i - 1)*2 + 1]
							),
						new Vector2(
							SCOffsets[(i - 1)*2],
							SCOffsets[(i - 1)*2 + 1]
							),
						MoveMargin))
			{
				continue ;
			}
			else
			{
				return true;
			}
		}
		return false;
	}

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
		if (MeshScenesAmt >= 2)
		{
			UnsolveMovement();
			GD.Print("Congratulations! Everyone (", MeshScenesAmt, ") is in correct rotation. Begin pos check");
			if (AtLeastOneOffsetBad())
			{
				GD.Print("Whoops, nope");
				return ;
			}
			GD.Print("You're within marings!");
			MovementSolved();
		}
		else
		{
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
		RealOffsetArray = new float[MeshScenesAmt*2];
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
		int localIndex = 0;
		foreach (ShadowCaster LocalNode in GetChildren())
		{
			LocalNode.SolutionFinalized = true;
			LocalNode.SetRotTgtToClosestTgt();
			if (localIndex != 0)
			{
				LocalNode.MovTargetReal = ((ShadowCaster)GetChildren()[0]).GlobalPosition + new Vector3(
						SCOffsets[(localIndex - 1)*2],
						SCOffsets[(localIndex - 1)*2 + 1],
						0.0f);
			}
			localIndex++;
		}
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
		RotationCloseness = new float[MeshScenesAmt];
		if (MeshScenesAmt > 1)
		{
			MoveCloseness = new float[MeshScenesAmt - 1];
		}
		if (!Visible)
			return ;
		LoadLevel();
	}

	private void UnsolveMovement()
	{
		InMovementSolution = false;
		GD.Print("Unsolved movement...");
	}

	public override void _Process(double delta)
	{
		if (Visible && MeshScenesAmt >= 2)
		{
			for (int i = 1; i < MeshScenesAmt; i++)
			{
				RealOffsetArray[(i - 1)*2] = ((CharacterBody3D)GetChildren()[i]).GlobalPosition.X - ((CharacterBody3D)GetChildren()[0]).GlobalPosition.X;
				RealOffsetArray[(i - 1)*2 + 1] = ((CharacterBody3D)GetChildren()[i]).GlobalPosition.Y - ((CharacterBody3D)GetChildren()[0]).GlobalPosition.Y;
			}
			if (InMovementSolution && AtLeastOneOffsetBad())
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
