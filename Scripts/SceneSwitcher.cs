using Godot;
using System;
using System.IO;
using System.Text;

public partial class SceneSwitcher : Node
{
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

	private void BackToLevels()
	{
		ChangeSceneToPath("res://Scenes/BeautifulLevelMenu.tscn");
	}

	private void NewGame()
	{
		Settings.Instance.DevMode = false;
		GD.Print("This should be a regular game!");
		GD.Print("Attempting to create a savefile...");
		if (!File.Exists(Settings.Instance.SavePath))
		{
			try
			{
				using (FileStream fs = File.Create(Settings.Instance.SavePath))
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
			Settings.Instance.CompletionShown = new bool[Settings.Instance.LevelNames.Length];
			Settings.Instance.UnlockShown = new bool[Settings.Instance.LevelNames.Length];
			Settings.Instance.MaxAvailableLevel = 0;
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
			using (FileStream fs = File.Create(Settings.Instance.SavePath))
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
		Settings.Instance.MaxAvailableLevel = 0;
		Settings.Instance.CompletionShown = new bool[Settings.Instance.LevelNames.Length];
		Settings.Instance.UnlockShown = new bool[Settings.Instance.LevelNames.Length];
		ChangeSceneToPath("res://Scenes/BeautifulLevelMenu.tscn");
	}

	private void LoadGame()
	{
		Settings.Instance.DevMode = false;
		GD.Print("This should be a load of the game");
		GD.Print("Attempting to read savefile...");
		try
		{
			var localText = File.ReadAllText(Settings.Instance.SavePath);
			int localLevel = 0;

			if (Int32.TryParse(localText, out localLevel))
			{
				GD.Print("Succeeded parsing level number: ", localLevel);
				Settings.Instance.MaxAvailableLevel = localLevel;
			}
			else
			{
				GD.Print("Level parsing failed");
				Settings.Instance.MaxAvailableLevel = 0;
			}
		}
		catch (Exception e)
		{
			GD.Print("Failed in reading a savefile:", e.Message);
			// TODO go into "savefileless" mode
		}
		ChangeSceneToPath("res://Scenes/BeautifulLevelMenu.tscn");
	}

	private void DevGame()
	{
		Settings.Instance.DevMode = true;
		ChangeSceneToPath("res://Scenes/BeautifulLevelMenu.tscn");
		GD.Print("Dev mode enabled!");
	}

	private void PrepareAndEnter(int WhichLevel)
	{
		Settings.Instance.YouNeedThisLevel = WhichLevel;
		ChangeSceneToPath("res://Scenes/Main3D.tscn");
		GD.Print("Trying to access level: ", WhichLevel);
	}

	private void SaveProgress(int WhichLevel)
	{
		if (Settings.Instance.DevMode)
		{
			return ;
		}
		GD.Print("saving progress! WL: ", WhichLevel);
		if (WhichLevel + 1 <= Settings.Instance.MaxAvailableLevel)
		{
			GD.Print("nevermind. SIMAL: ", Settings.Instance.MaxAvailableLevel);
			return ;
		}
		WhichLevel += 1;
		try
		{
			using (FileStream fs = File.Create(Settings.Instance.SavePath))
			{
				byte[] info = new UTF8Encoding(true).GetBytes(WhichLevel.ToString());
				fs.Write(info, 0, info.Length);
			}
		}
		catch (Exception e)
		{
			GD.Print("Failed in rewriting a savefile:", e.Message);
		}
	}

	private void UpdateMaxLvl(int WhichLevel)
	{
		if (Settings.Instance.MaxAvailableLevel < WhichLevel + 1)
		{
			Settings.Instance.MaxAvailableLevel = WhichLevel + 1;
		}
	}

	public override void _Ready()
	{
		NodeOfSignals = GetNode<Signals>("/root/Signals");
		Signals.Instance.SayYesToBacking += BackToMenu;
		Signals.Instance.ReturnFromFinished += BackToMenu;
		Signals.Instance.BackToLevels += BackToLevels;
		Signals.Instance.BackFromLevels += BackToMenu;
		Signals.Instance.NewGame += NewGame;
		Signals.Instance.LoadGame += LoadGame;
		Signals.Instance.NewDevGame += DevGame;
		Signals.Instance.PrepareLevel += PrepareAndEnter;
		Signals.Instance.ConfirmOverwrite += OverwriteNewGame;
		Signals.Instance.LevelFinished += SaveProgress;
		Signals.Instance.LevelFinished += UpdateMaxLvl;
	}
}
