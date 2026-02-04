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
}
