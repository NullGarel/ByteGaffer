using Godot;
using Godot.Collections;
using System;
using System.Linq;

namespace NullGarel.ByteGaffer;

[GlobalClass]
public partial class HexTranscoder : BaseTranscoder
{
    public override bool IsLossless => true;

    [Export] public bool UpperCase { get; set; } = false;
    [Export] public bool Spacing { get; set; } = false;
    [Export] public bool PrefixWith0x { get; set; } = false;

    public override string Encode(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);

        string format = UpperCase ? "X2" : "x2";

        if (Spacing)
        {
            string[] hexBytes = new string[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
                hexBytes[i] = $"{(PrefixWith0x ? "0x" : "")}{bytes[i].ToString(format)}";

            string result = string.Join(' ', hexBytes);

            return result;
        }
        else
        {
            string result = Convert.ToHexString(bytes);

            if (!UpperCase)
                result = result.ToLowerInvariant();

            return $"{(PrefixWith0x ? "0x" : "")}{result}";
        }
    }

    public override string Decode(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        string hex = input.Trim();
        hex = string.Concat(hex.Where(c => !char.IsWhiteSpace(c)));
        hex = hex.Replace("0x", "", StringComparison.OrdinalIgnoreCase);

        if (hex.Length % 2 != 0)
            throw new FormatException("Hex input must contain an even number of characters.");

        try
        {
            byte[] bytes = Convert.FromHexString(hex);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        catch (FormatException)
        {
            throw new FormatException("Input contains invalid hexadecimal characters.");
        }
    }
}