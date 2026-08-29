using Godot;
using System;
using System.Linq;
using System.Text;

namespace NullGarel.ByteGaffer;

[GlobalClass]
public partial class BinaryTranscoder : BaseTranscoder
{
    public override bool IsLossless => true;

    public override string Encode(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        byte[] bytes = Encoding.UTF8.GetBytes(input);

        return string.Join(" ", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
    }

    public override string Decode(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        string[] binarySegments = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        byte[] bytes = new byte[binarySegments.Length];

        for (int i = 0; i < binarySegments.Length; i++)
        {
            bytes[i] = Convert.ToByte(binarySegments[i], 2);
        }

        return Encoding.UTF8.GetString(bytes);
    }
}
