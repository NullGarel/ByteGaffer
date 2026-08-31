/*
*   20260831
*   DISCLAIMER: CLANKER GENERATED/REWRITTEN. POTENTIALLY SLOPPY CODE.
*   AGENT: CLAUDE.AI SONNET 5
*/
using System;
using System.Collections.Generic;
using System.Text;
using Godot;
namespace NullGarel.ByteGaffer;

[GlobalClass]
public partial class MorseCodeTranscoder : BaseTranscoder
{
    public override bool IsLossless => false;

    // Forward map: every character (including accented variants) to its Morse code.
    // Several accented letters intentionally share the same code as their base
    // letter (e.g. á/â/ã/à -> "."); which character wins on decode is NOT decided
    // by dictionary iteration order anymore (that was fragile) - see _morseToChar.
    private static readonly Dictionary<char, string> _charToMorse = new()
    {
        {'á', ".-"}, {'â', ".-"}, {'ã', ".-"}, {'à', ".-"}, {'a', ".-"},
        {'b', "-..."},
        {'c', "-.-."},
        {'ç', "-.-.."},
        {'d', "-.."},
        {'ê', "..-.."}, {'ẽ', "..-.."}, {'é', "..-.."},
        {'e', "."},
        {'f', "..-."},
        {'g', "--."},
        {'h', "...."},
        {'í', ".."}, {'ì', ".."}, {'ĩ', ".."}, {'î', ".."},
        {'i', ".."},
        {'j', ".---"},
        {'k', "-.-"},
        {'l', ".-.."},
        {'m', "--"},
        {'ñ', "--.--"},
        {'n', "-."},
        {'ó', "---"}, {'õ', "---"}, {'ô', "---"}, {'ò', "---"},
        {'o', "---"},
        {'p', ".--."},
        {'q', "--.-"},
        {'r', ".-."},
        {'s', "..."},
        {'t', "-"},
        {'ú', "..-"}, {'ù', "..-"}, {'ũ', "..-"}, {'û', "..-"},
        {'u', "..-"},
        {'v', "...-"},
        {'w', ".--"},
        {'x', "-..-"},
        {'y', "-.--"},
        {'z', "--.."},

        {'1', ".----"},
        {'2', "..---"},
        {'3', "...--"},
        {'4', "....-"},
        {'5', "....."},
        {'6', "-...."},
        {'7', "--..."},
        {'8', "---.."},
        {'9', "----."},
        {'0', "-----"},

        {'.', ".-.-.-"},
        {',', "--..--"},
        {'?', "..--.."},
        {'\'', ".----."},
        {'!', "-.-.--"},
        {'/', "-..-."},
        {'(', "-.--."},
        {')', "-.--.-"},
        {'&', ".-..."},
        {':', "---..."},
        {';', "-.-.-."},
        {'=', "-...-"},
        {'+', ".-.-."},
        {'-', "-....-"},
        {'_', "..--.-"},
        {'"', ".-..-."},
        {'$', "...-..-"},
        {'@', ".--.-."},

        {' ', "/"}
    };

    // Reverse map: explicit and unambiguous. Where several letters share a code
    // (a/á/â/ã/à, i/í/ì/ĩ/î, o/ó/õ/ô/ò, u/ú/ù/ũ/û) we deliberately decode to the
    // base, unaccented letter. Do NOT generate this from _charToMorse - iteration
    // order over a Dictionary<char,string> is an implementation detail, not a
    // guarantee, so relying on "last write wins" during a loop is fragile.
    private static readonly Dictionary<string, char> _morseToChar = new()
    {
        {".-", 'a'},
        {"-...", 'b'},
        {"-.-.", 'c'},
        {"-.-..", 'ç'},
        {"-..", 'd'},
        {"..-..", 'é'},
        {".", 'e'},
        {"..-.", 'f'},
        {"--.", 'g'},
        {"....", 'h'},
        {"..", 'i'},
        {".---", 'j'},
        {"-.-", 'k'},
        {".-..", 'l'},
        {"--", 'm'},
        {"--.--", 'ñ'},
        {"-.", 'n'},
        {"---", 'o'},
        {".--.", 'p'},
        {"--.-", 'q'},
        {".-.", 'r'},
        {"...", 's'},
        {"-", 't'},
        {"..-", 'u'},
        {"...-", 'v'},
        {".--", 'w'},
        {"-..-", 'x'},
        {"-.--", 'y'},
        {"--..", 'z'},

        {".----", '1'},
        {"..---", '2'},
        {"...--", '3'},
        {"....-", '4'},
        {".....", '5'},
        {"-....", '6'},
        {"--...", '7'},
        {"---..", '8'},
        {"----.", '9'},
        {"-----", '0'},

        {".-.-.-", '.'},
        {"--..--", ','},
        {"..--..", '?'},
        {".----.", '\''},
        {"-.-.--", '!'},
        {"-..-.", '/'},
        {"-.--.", '('},
        {"-.--.-", ')'},
        {".-...", '&'},
        {"---...", ':'},
        {"-.-.-.", ';'},
        {"-...-", '='},
        {".-.-.", '+'},
        {"-....-", '-'},
        {"..--.-", '_'},
        {".-..-.", '"'},
        {"...-..-", '$'},
        {".--.-.", '@'},

        {"/", ' '}
    };

    // Prosigns are matched case-insensitively against the input, then always
    // rendered back as their canonical upper-case form on decode.
    private static readonly List<KeyValuePair<string, string>> _prosigns = new()
    {
        new("[CQ]", "-.-.--.-"),
        new("[SOS]", "...---..."),
        new("[HH]", "........"),
    };

    private static readonly Dictionary<string, string> _reverseProsigns;

    static MorseCodeTranscoder()
    {
        _reverseProsigns = [];
        foreach (var kvp in _prosigns)
        {
            // NOTE: [BT]'s code ("-...-") collides with the character '='.
            // Prosigns are checked before letters on decode, so that sequence
            // always decodes as [BT]. This mirrors the original behaviour.
            _reverseProsigns[kvp.Value] = kvp.Key;
        }
    }

    public override string Encode(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        string lowerInput = input.ToLowerInvariant();
        StringBuilder sb = new();

        int i = 0;
        while (i < lowerInput.Length)
        {
            bool matchedProsign = false;

            if (lowerInput[i] == '[')
            {
                ReadOnlySpan<char> remaining = lowerInput.AsSpan(i);
                foreach (var kvp in _prosigns)
                {
                    if (remaining.StartsWith(kvp.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        sb.Append(kvp.Value).Append(' ');
                        i += kvp.Key.Length;
                        matchedProsign = true;
                        break;
                    }
                }
            }

            if (!matchedProsign)
            {
                char c = lowerInput[i];
                if (_charToMorse.TryGetValue(c, out string code))
                {
                    sb.Append(code).Append(' ');
                }
                // Unrecognized characters are silently skipped (lossy by design).
                i++;
            }
        }

        // Trim a single trailing separator, if any, without rebuilding the string.
        int len = sb.Length;
        if (len > 0 && sb[len - 1] == ' ')
            sb.Length = len - 1;

        return sb.ToString();
    }

    public override string Decode(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        StringBuilder sb = new();
        string[] words = input.Trim().Split(
            [" / ", " /", "/ ", "/"],
            StringSplitOptions.None);

        for (int i = 0; i < words.Length; i++)
        {
            string[] letters = words[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string letter in letters)
            {
                if (_reverseProsigns.TryGetValue(letter, out string prosign))
                {
                    sb.Append(prosign);
                }
                else if (_morseToChar.TryGetValue(letter, out char c))
                {
                    sb.Append(c);
                }
                // Unrecognized Morse tokens are silently skipped (lossy by design).
            }

            if (i < words.Length - 1)
            {
                sb.Append(' ');
            }
        }

        return sb.ToString();
    }
}