using System.Linq;
using System.Text;
using Godot;
using Godot.Collections;
namespace NullGarel.ByteGaffer;

[GlobalClass]
public partial class MorseCodeTranscoder : BaseTranscoder
{
    public override bool IsLossless => false;

    private readonly Dictionary<char, string> _morse = new()
    {
        {'a', ".-"},
        {'b', "-..."},
        {'c', "-.-."},
        {'ç', "-.-.."},
        {'d', "-.."},
        {'e', "."},
        {'é', "..-.."},
        {'f', "..-."},
        {'g', "--."},
        {'h', "...."},
        {'i', ".."},
        {'j', ".---"},
        {'k', "-.-"},
        {'l', ".-.."},
        {'m', "--"},
        {'n', "-."},
        {'o', "---"},
        {'p', ".--."},
        {'q', "--.-"},
        {'r', ".-."},
        {'s', "..."},
        {'t', "-"},
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

    private readonly Dictionary<string, string> _prosigns = new()
    {
        {"[CQ]", "-.-.--.-"},
        {"[SOS]", "...---..."},
        {"[AR]", ".-.-."},
        {"[SK]", "...-.-"},
        {"[BT]", "-...-"},
        {"[HH]", "........"},
        {"[KN]", "-.--."}
    };

    private Dictionary<string, string> _reverseProsigns;
    private Dictionary<string, char> _reverseMorse;

    public MorseCodeTranscoder()
    {
        _reverseMorse = [];
        foreach (var kvp in _morse)
        {
            _reverseMorse[kvp.Value] = kvp.Key;
        }

        _reverseProsigns = [];
        foreach (var kvp in _prosigns)
        {
            _reverseProsigns[kvp.Value] = kvp.Key;
        }
    }

    public override string Encode(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        string lowerInput = input.ToLower();
        StringBuilder sb = new();

        int i = 0;
        while (i < lowerInput.Length)
        {
            bool matchedProsign = false;

            if (lowerInput[i] == '[')
            {
                foreach (var kvp in _prosigns)
                {
                    if (lowerInput.Substring(i).StartsWith(kvp.Key))
                    {
                        sb.Append(kvp.Value);
                        sb.Append(" ");
                        i += kvp.Key.Length;
                        matchedProsign = true;
                        break;
                    }
                }
            }

            if (!matchedProsign)
            {
                char c = lowerInput[i];
                if (_morse.TryGetValue(c, out string code))
                {
                    sb.Append(code);
                    sb.Append(" ");
                }
                i++;
            }
        }

        return sb.ToString().TrimEnd();
    }

    public override string Decode(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        StringBuilder sb = new();
        string[] words = input.Trim().Split([" / ", " /", "/ ", "/"], System.StringSplitOptions.None);

        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];
            string[] letters = word.Split([' '], System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string letter in letters)
            {
                if (_reverseProsigns.TryGetValue(letter, out string prosign))
                {
                    sb.Append(prosign);
                }
                else if (_reverseMorse.TryGetValue(letter, out char c))
                {
                    sb.Append(c);
                }
            }

            if (i < words.Length - 1)
            {
                sb.Append(' ');
            }
        }

        return sb.ToString();
    }
}