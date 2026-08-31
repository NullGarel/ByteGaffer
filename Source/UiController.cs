using System.Linq;
using Godot;
using Godot.Collections;
namespace NullGarel.ByteGaffer;

public partial class UiController : Node
{
	[Export] public TranscodingManager TranscodingManager { get; set; }
	[Export] public OptionButton TranscodersDisplay { get; set; }
	[Export] private TextEdit _encodeInput;
	[Export] private TextEdit _decodeInput;

	public override void _Ready()
	{
		PopulateTranscodersDisplay();

		ConnectUiSignals();
	}

	private void ConnectUiSignals()
	{
		TranscodersDisplay.ItemSelected += id =>
		{
			TranscodingManager.SetTranscoderById((string)TranscodersDisplay.GetItemMetadata((int)id));
		};

		_encodeInput.TextChanged += OnEncodePressed;
		_decodeInput.TextChanged += OnDecodePressed;
		//encoding is the default one, might change tho.
		TranscodersDisplay.ItemSelected += (_) => OnEncodePressed();
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
		TranscodingManager.ExecuteDecoding(ref _encodeInput, ref _decodeInput);
	}

	private void OnEncodePressed()
	{
		TranscodingManager.ExecuteEncoding(ref _encodeInput, ref _decodeInput);
	}
}

