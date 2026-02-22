using Godot;
using System;

public partial class SelectObjectController : VBoxContainer
{
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
		Signals.Instance.LevelLoaded += OnLevelLoaded;
	}

	public override void _ExitTree()
	{
		Signals.Instance.LevelLoaded -= OnLevelLoaded;
	}
}
