using System;
using System.Collections.Generic;
using System.Linq;

public class SimplePathResolver
{
    //(child/child) - transform.Find(path)
    //<type> - GetComponents(typeof(Component))
    //. - field or prop accessor name
    private const char ObjectAccessorLex = '.';

    private static readonly List<CharPair> pairs = new List<CharPair>
    {
        new CharPair {StartChar = '(', EndChar = ')'},
        new CharPair
        {
            StartChar = '[', EndChar = ']',
            IgnoreTrailingWhiteSpace = true,
            ValidateFirstLetter = c => char.IsLetter(c),
            ValidateLetter = c => char.IsLetterOrDigit(c)
        }
    };

    private static Dictionary<char, char> Pairs = new Dictionary<char, char>
    {
        {'[', ']'},
        {'(', ')'}
    };

    private static readonly char[] whitespaces = { ' ', '\t', '\n', '\r' };
    //    public static object Resolve(string path) { }
    //    public static string GetAccessor() { }
    //rules:
    //[only valid type names]
    //should end with a valid type name ex:()[].
    //brackets should match
    //type and accessors can start with letters only
    //should end with an accessor
    public static bool Validate(string path)
    {
        var index = 0;
        char cChar;
        path = path.Trim();

        if (!char.IsLetterOrDigit(path[path.Length - 1]))
            return false;

        

        CharPair currentCharPair = null;
        var isLiteral = false;

        for (var i = 0; i < path.Length; i++)
        {
            cChar = path[i];

            if (currentCharPair == null)
            {
                if (IsWhiteSpace(cChar)) continue;
                var pair = pairs.FirstOrDefault(x => x.StartChar == cChar);
                if (pair != null)
                {
                    currentCharPair = pair;
                    if (currentCharPair.IgnoreTrailingWhiteSpace)
                        for (var j = i; j < path.Length; j++)
                        {
                            if (IsWhiteSpace(cChar)) continue;
                            i = j;
                            break;
                        }
                    if (i >= path.Length) //we skiped whitespaces till the end of string
                        return false;

                    if (!pair.ValidateFirstLetter(cChar))
                        return false;
                }
            }
            else
            {
                if (!currentCharPair.Validate(cChar))
                    return false;
                if (currentCharPair.isClosed)
                    currentCharPair = null;
                //should be a literal
            }
        }
        return currentCharPair == null;
    }

    private static bool IsWhiteSpace(char c)
    {
        return whitespaces.Contains(c);
    }

    private class CharPair
    {
        public char StartChar;
        public char EndChar;
        public bool IgnoreTrailingWhiteSpace;
        public bool isClosed;
        public Func<char, bool> ValidateFirstLetter = c => true;

        public Func<char, bool> ValidateLetter = c => true;

        public bool Validate(char c)
        {
            if (c == EndChar) return true;
            return ValidateLetter(c);
        }
    }
}