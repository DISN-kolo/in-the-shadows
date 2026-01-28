using Godot;
using System;

public partial class DropdownLevel : OptionButton
{

	private Signals NodeOfSignals;

	public void OnDropdownLevelAsked(long Index)
	{
		NodeOfSignals.EmitSignal(Signals.SignalName.AskToChangeLevel, (int)Index);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		ItemSelected += OnDropdownLevelAsked;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
