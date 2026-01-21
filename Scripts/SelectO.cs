using Godot;
using System;

public partial class SelectO : CheckButton
{
	private Signals NodeOfSignals;

	[Export]
	public int Number = 0;

	private void OnPressed()
	{
		bool ToggledOn = this.ButtonPressed;
		if (ToggledOn)
		{
			NodeOfSignals.EmitSignal(Signals.SignalName.ActivateObject, this.Number);
		}
		else
		{
			if (this.Number == 0)
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
		if (RecNumber == this.Number)
		{
			this.ButtonPressed = true;
		}
		else
		{
			this.ButtonPressed = false;
		}
	}

	private void OnLevelLoaded(int MeshesAmt)
	{
		if (MeshesAmt - 1 >= this.Number)
		{
			this.Visible = true;
		}
		else
		{
			this.Visible = false;
		}
		if (this.Number == 0)
		{
			this.ButtonPressed = true;
		}
		else
		{
			this.ButtonPressed = false;
		}
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		this.Pressed += OnPressed;
		Signals.Instance.ActivateObject += OnToggledOtherSelector;
		Signals.Instance.LevelLoaded += OnLevelLoaded;
	}

	public override void _Process(double delta)
	{
	}
}
