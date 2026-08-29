using Godot;
using System;
namespace NullGarel.ByteGaffer;

/// <summary>
/// API that sits in between the UI and the transcoders.
/// A transcoder is a name I made up for something that both encodes and decodes.
/// </summary>
[GlobalClass]
public partial class TranscodingManager : Node
{
    [Export] private TextEdit EncodeInput { get; set; }
    [Export] private TextEdit DecodeInput { get; set; }
    [Export] private BaseTranscoder CurrentTranscoder { get; set; }

    public void ExecuteEncoding()
    {
        DecodeInput.Text = CurrentTranscoder.Encode(EncodeInput.Text);
    }

    public void ExecuteDecoding()
    {
        EncodeInput.Text = CurrentTranscoder.Decode(DecodeInput.Text);
    }
}