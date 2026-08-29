using Godot;
using System;
namespace NullGarel.ByteGaffer;

public partial class UiController : Node
{
	[Export] public Button EncodeButton { get; set; }
	[Export] public Button DecodeButton { get; set; }
	[Export] public TranscodingManager TranscodingManager { get; set; }


	public override void _Ready()
	{
		ConnectUiSignals();
	}

	private void ConnectUiSignals()
	{
		EncodeButton.Pressed += OnEncodePressed;
		DecodeButton.Pressed += OnDecodePressed;
	}

	private void OnDecodePressed()
	{
		TranscodingManager.ExecuteDecoding();
	}

	private void OnEncodePressed()
	{
		TranscodingManager.ExecuteEncoding();
	}
}
