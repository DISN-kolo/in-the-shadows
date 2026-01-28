using Godot;
using System;

public partial class SelectObjectController : VBoxContainer
{
	private Signals NodeOfSignals;

	private void OnLevelLoaded(int MeshesAmt)
	{
		if (MeshesAmt <= 1)
		{
			Visible = false;
		}
		else
		{
			Visible = true;
		}
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		Signals.Instance.LevelLoaded += OnLevelLoaded;
	}

	public override void _Process(double delta)
	{
	}
}
