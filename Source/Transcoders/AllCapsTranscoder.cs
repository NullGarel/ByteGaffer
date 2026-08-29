using Godot;
using System;
namespace NullGarel.ByteGaffer;

[GlobalClass]
public partial class AllCapsTranscoder : BaseTranscoder
{
    public override bool IsLossless => false;

    public override string Encode(string input) => input.ToUpperInvariant();
    
    public override string Decode(string input) => input.ToLowerInvariant();

}