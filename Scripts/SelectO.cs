using Godot;
using System;

public partial class SelectO : CheckButton
{
	private Signals NodeOfSignals;

	[Export]
	public int Number = 0;

	private void OnPressed()
	{
		bool ToggledOn = ButtonPressed;
		if (ToggledOn)
		{
			NodeOfSignals.EmitSignal(Signals.SignalName.ActivateObject, Number);
		}
		else
		{
			if (Number == 0)
			{
				NodeOfSignals.EmitSignal(Signals.SignalName.ActivateObject, 1);
			}
			else
			{
				NodeOfSignals.EmitSignal(Signals.SignalName.ActivateObject, 0);
			}
		}
	}

	private void OnToggledOtherSelector(int RecNumber)
	{
		if (RecNumber == Number)
		{
			ButtonPressed = true;
		}
		else
		{
			ButtonPressed = false;
		}
	}

	private void OnLevelLoaded(int MeshesAmt)
	{
		if (MeshesAmt - 1 >= Number)
		{
			Visible = true;
		}
		else
		{
			Visible = false;
		}
		if (Number == 0)
		{
			ButtonPressed = true;
		}
		else
		{
			ButtonPressed = false;
		}
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		Pressed += OnPressed;
		Signals.Instance.ActivateObject += OnToggledOtherSelector;
		Signals.Instance.LevelLoaded += OnLevelLoaded;
	}

	public override void _ExitTree()
	{
		Pressed -= OnPressed;
		Signals.Instance.ActivateObject -= OnToggledOtherSelector;
		Signals.Instance.LevelLoaded -= OnLevelLoaded;
	}
}
