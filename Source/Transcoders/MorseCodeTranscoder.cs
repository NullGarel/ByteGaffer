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
        {'d', "-.."},
        {'e', "."},
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
        {' ', "/"}
    };

    private Dictionary<string, char> _reverseMorse;

    public MorseCodeTranscoder()
    {
        _reverseMorse = [];
        foreach (var kvp in _morse)
        {
            _reverseMorse[kvp.Value] = kvp.Key;
        }
    }

    public override string Encode(string input)
    {
        StringBuilder sb = new();
        foreach (char c in input.ToLower())
        {
            if (_morse.TryGetValue(c, out string code))
            {
                sb.Append(code);
                sb.Append(" ");
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
                if (_reverseMorse.TryGetValue(letter, out char c))
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