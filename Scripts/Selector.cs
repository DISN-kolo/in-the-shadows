using Godot;
using System;

public partial class Selector : VBoxContainer
{
	private Signals NodeOfSignals;
	private Button SelectorButton;
	private Label SelectorLabel;
	private TextureRect WinGreen;
	private AnimationPlayer WinAnimPlayer;
	private ShaderMaterial BtnShaderMat;
	private ShaderMaterial LblShaderMat;

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
		// why not apply these material-related things in the nodes themselves?
		// well, I don't want to make 2 more scripts just for that. also, the spawn order and
		//container-based sizing/positioning might be affected by the load process
		SelectorButton = GetNode<Button>("VBoxSelector/SelectorButton");
		BtnShaderMat = (ShaderMaterial)(SelectorButton.GetMaterial());
		SelectorLabel = GetNode<Label>("VBoxSelector/SelectorLabel");
//		LblShaderMat = (ShaderMaterial)(SelectorLabel.GetMaterial());

		BtnShaderMat.SetShaderParameter("max_x", SelectorButton.Size.X);
		BtnShaderMat.SetShaderParameter("max_y", SelectorButton.Size.Y);
//		LblShaderMat.SetShaderParameter("max_x", SelectorLabel.Size.X);
//		LblShaderMat.SetShaderParameter("max_y", SelectorLabel.Size.Y);

		WinGreen = GetNode<TextureRect>("VBoxSelector/SelectorButton/WinGreen");
		WinAnimPlayer = GetNode<AnimationPlayer>("VBoxSelector/SelectorButton/WinAnimPlayer");
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
			SelectorLabel.Text = "???";
		}
		else
		{
			SelectorLabel.Text = Settings.Instance.LevelNames[LevelNumber];
		}
	}

	public override void _ExitTree()
	{
		SelectorButton.Pressed -= OnPressed;
	}
}
