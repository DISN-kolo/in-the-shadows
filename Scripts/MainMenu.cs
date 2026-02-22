using Godot;
using System;
using System.IO;

public partial class MainMenu : Control
{
	private PackedScene OvPromptScene;
	private AreYouOverwriting OvPromptInstance;

	private void SpawnOvPromptWindow()
	{
		OvPromptInstance = (AreYouOverwriting)OvPromptScene.Instantiate();
		OvPromptInstance.GlobalPosition = GetViewport().GetVisibleRect().Size;
		OvPromptInstance.GlobalPosition = OvPromptInstance.GlobalPosition with { X = OvPromptInstance.GlobalPosition.X / 2, Y = OvPromptInstance.GlobalPosition.Y / 2 };
		AddChild(OvPromptInstance);
	}

	public override void _Ready()
	{
		OvPromptScene = GD.Load<PackedScene>("res://Scenes/AreYouOverwriting.tscn");
		Signals.Instance.PromptOverwrite += SpawnOvPromptWindow;
		if (File.Exists(Settings.Instance.SavePath) || Settings.Instance.MaxAvailableLevel != 0)
		{
			GetNode<Button>("Centrerer/StuffVBox/Container/VBoxContainer/LoadGameButton").Disabled = false;
		}
	}

	public override void _ExitTree()
	{
		Signals.Instance.PromptOverwrite -= SpawnOvPromptWindow;
	}
}
