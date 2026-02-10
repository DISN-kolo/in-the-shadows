using Godot;
using System;

public partial class Selector : VBoxContainer
{
	private Signals NodeOfSignals;
	private Button SelectorButton;
	private TextureRect WinGreen;

	private bool CompletionShown;

	[Export]
	public int LevelNumber = 0;

	[Export]
	public string DesiredLabel = "";

	private void OnPressed()
	{
		NodeOfSignals.EmitSignal(Signals.SignalName.PrepareLevel, LevelNumber);
	}

	public override void _Ready()
	{
		SelectorButton = GetNode<Button>("SelectorButton");
		WinGreen = GetNode<TextureRect>("SelectorButton/WinGreen");
		SelectorButton.Icon = GD.Load<Texture2D>($"res://Icons/iconsOfSin{LevelNumber + 1}.png");
		SelectorButton.Pressed += OnPressed;
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		if ((LevelNumber > Settings.Instance.MaxAvailableLevel) && (Settings.Instance.DevMode == false))
		{
			SelectorButton.Disabled = true;
			GetNode<Label>("SelectorLabel").Text = "???";
		}
		else
		{
			GetNode<Label>("SelectorLabel").Text = DesiredLabel;
		}
	}

	public override void _ExitTree()
	{
		SelectorButton.Pressed -= OnPressed;
	}
}
