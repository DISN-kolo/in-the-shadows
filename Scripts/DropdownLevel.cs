using Godot;
using System;

public partial class DropdownLevel : OptionButton
{

	private Signals NodeOfSignals;

	public void OnDropdownLevelAsked(long Index)
	{
		NodeOfSignals.EmitSignal(Signals.SignalName.AskToChangeLevel, (int)Index);
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		ItemSelected += OnDropdownLevelAsked;
	}

	public override void _ExitTree()
	{
		ItemSelected -= OnDropdownLevelAsked;
	}
}
