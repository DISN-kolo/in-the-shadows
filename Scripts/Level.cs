using Godot;
using System;

public partial class Level : Node3D
{
	[Export]
	public string[] MeshScenesPaths = {};

	// please note the linear array. it's actually {x, y, z, x, y, z...}
	[Export]
	public float[] SCRotations = {};

	// please note the linear array. it's actually {x, y, z, x, y, z...}
	// please also note that the amount of offsets is equal to the amount
	//of meshes - 1.
	[Export]
	public float[] SCOffsets = {};

	[Export]
	public bool AllowFlippedV = true;

	[Export]
	public bool AllowFlippedH = true;

	public PackedScene ShadowCasterScene;
	public Node CurrentShadowCasterInstance;

	public override void _Ready()
	{
		ShadowCasterScene = GD.Load<PackedScene>("res://Scenes/ShadowCaster.tscn");
		int MesheScenesAmt = MeshScenesPaths.Length;
		for (int i = 0; i < MeshesAmt; i++)
		{
			CurrentShadowCasterInstance = ShadowCasterScene.Instantiate();
			CurrentShadowCasterInstance.MeshScenePath = MeshScenesPaths[i];
			// TODO position like a cake
			AddChild(CurrentShadowCasterInstance);
		}
	}

	public override void _Process(double delta)
	{
	}
}
