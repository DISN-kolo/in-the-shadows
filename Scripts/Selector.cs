using Godot;
using System;

public partial class Selector : VBoxContainer
{
	private Signals NodeOfSignals;
	private Button SelectorButton;
	private TextureRect WinGreen;
	private AnimationPlayer WinAnimPlayer;

	[Export]
	public int LevelNumber = 0;

//	[Export]
//	public string DesiredLabel = "";

	private void OnPressed()
	{
		NodeOfSignals.EmitSignal(Signals.SignalName.PrepareLevel, LevelNumber);
	}

	public override void _Ready()
	{
		SelectorButton = GetNode<Button>("SelectorButton");
		WinGreen = GetNode<TextureRect>("SelectorButton/WinGreen");
		WinAnimPlayer = GetNode<AnimationPlayer>("SelectorButton/WinAnimPlayer");
		if (Settings.Instance.DevMode)
		{
			WinGreen.Visible = false;
		}
		else
		{
			if (LevelNumber < Settings.Instance.MaxAvailableLevel)
			{
				WinGreen.Visible = true;
				if (Settings.Instance.CompletionShown[LevelNumber])
				{
					WinAnimPlayer.SetCurrentAnimation("RESET");
				}
				else
				{
					WinAnimPlayer.SetCurrentAnimation("PlopDown");
					Settings.Instance.CompletionShown[LevelNumber] = true;
				}
			}
			else
			{
				WinGreen.Visible = false;
			}
		}
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
			GetNode<Label>("SelectorLabel").Text = Settings.Instance.LevelNames[LevelNumber];
		}
	}

	public override void _ExitTree()
	{
		SelectorButton.Pressed -= OnPressed;
	}
}
