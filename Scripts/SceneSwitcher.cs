using Godot;
using System;

public partial class SceneSwitcher : Node
{
	public void ChangeSceneToPath(string ScenePath)
	{
		GD.Print("received the following path: ", ScenePath);
		GetTree().ChangeSceneToFile(ScenePath);
		GD.Print("Alledgedly, changed!");
	}

	private void BackToMenu()
	{
		ChangeSceneToPath("res://Scenes/MainMenu.tscn");
	}

	private void NewGame()
	{
		ChangeSceneToPath("res://Scenes/Main3D.tscn");
		GD.Print("This should be a regular game!");
	}

	private void DevGame()
	{
		ChangeSceneToPath("res://Scenes/Main3D.tscn");
		GD.Print("Dev mode enabled!");
	}

	public override void _Ready()
	{
		Signals.Instance.SayYesToBacking += BackToMenu;
		Signals.Instance.NewGame += NewGame;
		Signals.Instance.NewDevGame += DevGame;
	}
}
