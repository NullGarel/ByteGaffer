using Godot;
using Godot.Collections;
using System;
using System.Linq;
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
    private BaseTranscoder _currentTranscoder;
    [Export]
    public BaseTranscoder CurrentTranscoder
    {
        get => _currentTranscoder;
        set
        {
            _currentTranscoder = value;
            TranscoderChanged?.Invoke(value.TranscoderId);
        }
    }
    [Export] public Array<BaseTranscoder> Transcoders { get; set; } = [];
    public event Action<string> TranscoderChanged;

    public void ExecuteEncoding()
    {
        DecodeInput.Text = _currentTranscoder.Encode(EncodeInput.Text);
    }

    public void ExecuteDecoding()
    {
        EncodeInput.Text = _currentTranscoder.Decode(DecodeInput.Text);
    }

    private void LoadTranscoders()
    {

        var transcoders = ResUtils.LoadResourcesFromFolder<BaseTranscoder>("res://Data/Transcoders/");

        Transcoders.AddRange(transcoders);
    }

    public void SetTranscoderById(string transcoderMetaId)
    {
        CurrentTranscoder = Transcoders.FirstOrDefault((tc) => tc.TranscoderId == transcoderMetaId);
    }

}