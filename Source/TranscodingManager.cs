using Godot;
using Godot.Collections;
namespace NullGarel.ByteGaffer;

/// <summary>
/// API that sits in between the UI and the transcoders.
/// A transcoder is a name I made up for something that both encodes and decodes.
/// </summary>
[GlobalClass]
public partial class TranscodingManager : Node
{
    public override void _EnterTree()
    {
        LoadTranscoders();
    }

    [Export] public TextEdit EncodeInput { get; set; }
    [Export] public TextEdit DecodeInput { get; set; }
    [Export] public BaseTranscoder CurrentTranscoder { get; set; }
    [Export] public Array<BaseTranscoder> Transcoders { get; set; } = [];

    public void ExecuteEncoding()
    {
        DecodeInput.Text = CurrentTranscoder.Encode(EncodeInput.Text);
    }

    public void ExecuteDecoding()
    {
        EncodeInput.Text = CurrentTranscoder.Decode(DecodeInput.Text);
    }

    private void LoadTranscoders()
    {

        var transcoders = ResUtils.LoadResourcesFromFolder<BaseTranscoder>("res://Data/Transcoders/");

        Transcoders.AddRange(transcoders);
    }

}