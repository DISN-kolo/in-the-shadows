using Godot;
using System;

public partial class Level : Node3D
{
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

	public override void _Ready()
	{
		if (!this.Visible)
			return ;
		ShadowCasterScene = GD.Load<PackedScene>("res://Scenes/ShadowCaster.tscn");
		MeshScenesAmt = MeshScenesPaths.Length;
		// TODO oob checks
		for (int i = 0; i < MeshScenesAmt; i++)
		{
			CurrentShadowCasterInstance = (ShadowCaster)ShadowCasterScene.Instantiate();
			CurrentShadowCasterInstance.MeshScenePath = MeshScenesPaths[i];
			CurrentShadowCasterInstance.IntendedRot = new Vector3(
				SCRotations[i*3] * (float)Math.PI,
				SCRotations[i*3 + 1] * (float)Math.PI,
				SCRotations[i*3 + 2] * (float)Math.PI
			);
			CurrentShadowCasterInstance.FlippableX = AllowFlippedVEx[i];
			CurrentShadowCasterInstance.FlippableY = AllowFlippedHEx[i];
			AddChild(CurrentShadowCasterInstance);
		}
	}

	public override void _Process(double delta)
	{
	}
}
