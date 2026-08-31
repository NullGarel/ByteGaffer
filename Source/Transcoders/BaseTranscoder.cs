using Godot;
using Godot.Collections;
using System;
namespace NullGarel.ByteGaffer;

[GlobalClass]
public abstract partial class BaseTranscoder : Resource
{
    [Export] public virtual string TranscoderId { get; set; }
    [Export] public virtual string TranscoderDisplayName { get; set; }

    public virtual bool IsLossless => true;
    /// <summary>
    /// Encodes into its own implementation
    /// </summary>
    /// <param name="input">text to be encoded</param>
    /// <returns>encoded text</returns>
    public abstract string Encode(string input);

    /// <summary>
    /// Decodes into a human readable whatever
    /// </summary>
    /// <param name="input">encoded text</param>
    /// <returns>decoded text</returns>
    public abstract string Decode(string input);
}