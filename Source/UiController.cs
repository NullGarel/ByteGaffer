using System.Linq;
using Godot;
using Godot.Collections;
namespace NullGarel.ByteGaffer;

public partial class UiController : Node
{
	[Export] public Button EncodeButton { get; set; }
	[Export] public Button DecodeButton { get; set; }
	[Export] public TranscodingManager TranscodingManager { get; set; }
	[Export] public OptionButton TranscodersDisplay { get; set; }


	public override void _Ready()
	{
		PopulateTranscodersDisplay();

		ConnectUiSignals();
	}

	private void ConnectUiSignals()
	{
		EncodeButton.Pressed += OnEncodePressed;
		DecodeButton.Pressed += OnDecodePressed;
		TranscodersDisplay.ItemSelected += id =>
		{
			TranscodingManager.CurrentTranscoder = TranscodingManager.Transcoders.FirstOrDefault((tc) => tc.TranscoderId == (string)TranscodersDisplay.GetItemMetadata((int)id));
		};
	}

	private void PopulateTranscodersDisplay()
	{
		TranscodersDisplay.Clear();

		for (int i = 0; i < TranscodingManager.Transcoders.Count; i++)
		{
			var t = TranscodingManager.Transcoders[i];

			TranscodersDisplay.AddItem(t.TranscoderDisplayName, i);
			TranscodersDisplay.SetItemMetadata(i, t.TranscoderId);
		}
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

