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

	public override void _Ready()
	{
		Signals.Instance.SayYesToBacking += BackToMenu;
	}
}
