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
		GD.Print("Just got onPressed from ", this);
		if (ToggledOn)
		{
			GD.Print("and we're YES toggled on");
			NodeOfSignals.EmitSignal(Signals.SignalName.ActivateObject, this.Number);
			GD.Print("signal emitted!");
		}
		else
		{
			GD.Print("NO we're toggled OFF");
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
		GD.Print("I, ", this, " just received: ", RecNumber, " while my own number was ", this.Number);
		if (RecNumber == this.Number)
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
		NodeOfSignals.ActivateObject += OnToggledOtherSelector;
	}

	public override void _Process(double delta)
	{
	}
}
