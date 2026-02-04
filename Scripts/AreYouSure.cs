using Godot;
using System;

public partial class AreYouSure : Control
{
	private void OnPressedNope()
	{
		QueueFree();
	}

	public override void _Ready()
	{
		Signals.Instance.SayNoToBacking += OnPressedNope;
	}
}
