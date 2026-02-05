using Godot;
using System;
using System.IO;
using System.Text;

public partial class SceneSwitcher : Node
{
	private string SavePath = "./savefile";
	private Signals NodeOfSignals;

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
		GD.Print("This should be a regular game!");
		GD.Print("Attempting to create a savefile...");
		if (!File.Exists(SavePath))
		{
			try
			{
				using (FileStream fs = File.Create(SavePath))
				{
					byte[] info = new UTF8Encoding(true).GetBytes("0");
					fs.Write(info, 0, info.Length);
				}
			}
			catch (Exception e)
			{
				GD.Print("Failed in creating a new savefile:", e.Message);
				// TODO go into "savefileless" mode
			}
		}
		else
		{
			GD.Print("Savefile alr exists");
			NodeOfSignals.EmitSignal(Signals.SignalName.PromptOverwrite);
			return ;
		}
		ChangeSceneToPath("res://Scenes/BeautifulLevelMenu.tscn");
	}

	private void OverwriteNewGame()
	{
		GD.Print("This should be a regular game, but with savefile overwrite");
		GD.Print("Attempting to create a savefile...");
		try
		{
			using (FileStream fs = File.Create(SavePath))
			{
				byte[] info = new UTF8Encoding(true).GetBytes("0");
				fs.Write(info, 0, info.Length);
			}
		}
		catch (Exception e)
		{
			GD.Print("Failed in creating a new savefile:", e.Message);
			// TODO go into "savefileless" mode
		}
		ChangeSceneToPath("res://Scenes/BeautifulLevelMenu.tscn");
	}

	private void DevGame()
	{
		ChangeSceneToPath("res://Scenes/BeautifulLevelMenu.tscn");
		GD.Print("Dev mode enabled!");
	}

	private void PrepareAndEnter(int WhichLevel)
	{
		Settings.Instance.YouNeedThisLevel = WhichLevel;
		ChangeSceneToPath("res://Scenes/Main3D.tscn");
		GD.Print("Trying to access level: ", WhichLevel);
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		Signals.Instance.SayYesToBacking += BackToMenu;
		Signals.Instance.NewGame += NewGame;
		Signals.Instance.NewDevGame += DevGame;
		Signals.Instance.PrepareLevel += PrepareAndEnter;
		Signals.Instance.ConfirmOverwrite += OverwriteNewGame;
	}
}
