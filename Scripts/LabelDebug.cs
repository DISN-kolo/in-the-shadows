using Godot;
using System;

public partial class LabelDebug : Label
{
	private bool SCLoaded = false;

	private CharacterBody3D ShadowCatcher;

	private void OnTreeExiting()
	{
//		ShadowCatcher = null;
//		SCLoaded = false;
	}

	private void OnFirstSpawned(CharacterBody3D SC)
	{
		GD.Print("oh, hello");
		ShadowCatcher = SC;
//		ShadowCatcher.TreeExiting += OnTreeExiting;
		SCLoaded = true;
		GD.Print("Showing SC: ", ShadowCatcher);
		GD.Print("its kids: ", ShadowCatcher.GetChildren());
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
//			this.Text = String.Format("rotx: {0,8:##0.00} | {1,8:##0.00}\n", ShadowCatcher.Rotation.X, ShadowCatcher.Rotation.X / Math.PI)
//				+ String.Format("roty: {0,8:##0.00} | {1,8:##0.00}\n", ShadowCatcher.Rotation.Y, ShadowCatcher.Rotation.Y / Math.PI)
//				+ String.Format("rotz: {0,8:##0.00} | {1,8:##0.00}\n", ShadowCatcher.Rotation.Z, ShadowCatcher.Rotation.Z / Math.PI);
			this.Text = String.Format("offx: {0,8:##0.00}\n", DebugSignals.OffsetCurrent.X)
				+ String.Format("offy: {0,8:##0.00}\n", DebugSignals.OffsetCurrent.Y)
				+ String.Format("offz: {0,8:##0.00}", DebugSignals.OffsetCurrent.Z);
		}
	}
}
