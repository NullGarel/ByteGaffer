using Godot;
using System;
using System.Text;
namespace NullGarel.ByteGaffer;

[GlobalClass]
public partial class Base64Transcoder : BaseTranscoder
{
    [Export] public bool UseZlib { get; set; } = false;

    public override bool IsLossless => false;

    public override string Encode(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        if (UseZlib)
        {
            bytes = ZlibPack.Pack(bytes);
        }
        return Convert.ToBase64String(bytes);
    }

    public override string Decode(string input)
    {
        byte[] bytes = Convert.FromBase64String(input);
        if (UseZlib)
        {
            bytes = ZlibPack.Unpack(bytes);
        }
        return Encoding.UTF8.GetString(bytes);
    }

}