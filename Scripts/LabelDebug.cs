using Godot;
using System;

public partial class LabelDebug : Label
{
	private bool SCLoaded = false;

	private CharacterBody3D ShadowCatcher;

	private void OnFirstSpawned(CharacterBody3D SC)
	{
		GD.Print("oh, hello");
		ShadowCatcher = SC;
		SCLoaded = true;
	}

	public override void _Ready()
	{
		GD.Print("Ready?");
		DebugSignals.Instance.FirstSpawned += OnFirstSpawned;
		GD.Print("Ready!");
	}

	public override void _Process(double delta)
	{
		if (SCLoaded)
		{
			this.Text = String.Format("rotx: {0,8:##0.00} | {1,8:##0.00}\n", ShadowCatcher.Rotation.X, ShadowCatcher.Rotation.X / Math.PI)
				+ String.Format("roty: {0,8:##0.00} | {1,8:##0.00}\n", ShadowCatcher.Rotation.Y, ShadowCatcher.Rotation.Y / Math.PI)
				+ String.Format("rotz: {0,8:##0.00} | {1,8:##0.00}\n", ShadowCatcher.Rotation.Z, ShadowCatcher.Rotation.Z / Math.PI);
		}
	}
}
