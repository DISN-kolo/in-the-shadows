using Godot;
using System;

public partial class Level : Node3D
{
	private Signals NodeOfSignals;

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

	public void LoadLevel()
	{
		MeshScenesAmt = MeshScenesPaths.Length;
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
			AddChild(CurrentShadowCasterInstance);
		}
		this.Visible = true;
		NodeOfSignals.EmitSignal(Signals.SignalName.LevelLoaded, MeshScenesAmt);
	}

	public void UnloadLevel()
	{
		// TODO disconnect signals
		foreach (var LocalNode in this.GetChildren())
		{
			LocalNode.QueueFree();
		}
		this.Visible = false;
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		ShadowCasterScene = GD.Load<PackedScene>("res://Scenes/ShadowCaster.tscn");
		if (!this.Visible)
			return ;
		LoadLevel();
	}

	public override void _Process(double delta)
	{
	}
}
