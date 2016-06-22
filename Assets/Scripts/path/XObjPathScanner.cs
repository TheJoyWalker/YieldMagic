/*using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.XPath;

public class XObjPathParser
{
    private LexKind kind;
    private string _expr;
    private char _cChar;
    private int _index;
    public string SourceText { get { return _expr; } }
    private char CurerntChar { get { return _cChar; } }
    public XObjPathParser(string expr)
    {
        if (expr == null)
        {
            throw new Exception();
        }
        _expr = expr;

        NextChar();
        NextLex();
    }
    private bool NextChar()
    {
        if (_index < _expr.Length)
        {
            _cChar = _expr[_index++];
            return true;
        }
        else {
            _cChar = '\0';
            return false;
        }
    }


    void SkipSpace()
    {
        while (IsWhiteSpace(this.CurerntChar) && NextChar()) ;
    }

    public bool NextLex()
    {
        SkipSpace();
        switch (this.CurerntChar)
        {
            case '\0':
                kind = LexKind.Eof;
                return false;
            case ',':
            case '@':
            case '#':
            case '$':
            case '(':
            case ')':
            case '[':
            case ']':
            case '|':
            case '*':
            case '+':
            case '-':
            case '=':
                kind = (LexKind)Convert.ToInt32(this.CurerntChar, CultureInfo.InvariantCulture);
                NextChar();
                break;
            case '<':
                kind = LexKind.Lt;
                NextChar();
                if (this.CurerntChar == '=')
                {
                    kind = LexKind.Le;
                    NextChar();
                }
                break;
            case '>':
                kind = LexKind.Gt;
                NextChar();
                if (this.CurerntChar == '=')
                {
                    kind = LexKind.Ge;
                    NextChar();
                }
                break;
            case '!':
                kind = LexKind.Bang;
                NextChar();
                if (this.CurerntChar == '=')
                {
                    kind = LexKind.Ne;
                    NextChar();
                }
                break;
            case '.':
                kind = LexKind.Dot;
                NextChar();
                if (this.CurerntChar == '.')
                {
                    kind = LexKind.DotDot;
                    NextChar();
                }
                else if (IsDigit(this.CurerntChar))
                {
                    kind = LexKind.Number;
                    numberValue = ScanFraction();
                }
                break;
            case '/':
                kind = LexKind.Slash;
                NextChar();
                if (this.CurerntChar == '/')
                {
                    kind = LexKind.SlashSlash;
                    NextChar();
                }
                break;
            case '"':
            case '\'':
                this.kind = LexKind.String;
                this.stringValue = ScanString();
                break;
            default:
                if (IsDigit(this.CurerntChar))
                {
                    kind = LexKind.Number;
                    numberValue = ScanNumber();
                }
                else if (xmlCharType.IsStartNCNameSingleChar(this.CurerntChar)
#if XML10_FIFTH_EDITION
                    || xmlCharType.IsNCNameHighSurrogateChar(this.CurerntChar) 
#endif
                    )
                {
                    kind = LexKind.Name;
                    this.name = ScanName();
                    this.prefix = string.Empty;
                    // "foo:bar" is one lexem not three because it doesn't allow spaces in between
                    // We should distinct it from "foo::" and need process "foo ::" as well
                    if (this.CurerntChar == ':')
                    {
                        NextChar();
                        // can be "foo:bar" or "foo::"
                        if (this.CurerntChar == ':')
                        {   // "foo::"
                            NextChar();
                            kind = LexKind.Axe;
                        }
                        else {                          // "foo:*", "foo:bar" or "foo: "
                            this.prefix = this.name;
                            if (this.CurerntChar == '*')
                            {
                                NextChar();
                                this.name = "*";
                            }
                            else if (xmlCharType.IsStartNCNameSingleChar(this.CurerntChar)
#if XML10_FIFTH_EDITION
                                || xmlCharType.IsNCNameHighSurrogateChar(this.CurerntChar)
#endif
                                )
                            {
                                this.name = ScanName();
                            }
                            else {
                                //                                throw XPathException.Create(Res.Xp_InvalidName, SourceText);
                            }
                        }

                    }
                    else {
                        SkipSpace();
                        if (this.CurerntChar == ':')
                        {
                            NextChar();
                            // it can be "foo ::" or just "foo :"
                            if (this.CurerntChar == ':')
                            {
                                NextChar();
                                kind = LexKind.Axe;
                            }
                            else {
                                //                                throw XPathException.Create(Res.Xp_InvalidName, SourceText);
                            }
                        }
                    }
                    SkipSpace();
                    this.canBeFunction = (this.CurerntChar == '(');
                }
                else {
                    //                    throw XPathException.Create(Res.Xp_InvalidToken, SourceText);
                }
                break;
        }
        return true;
    }

    #region Parse
    private float ScanNumber()
    {
        int start = _index - 1;
        int len = 0;
        while (IsDigit(this.CurerntChar))
        {
            NextChar(); len++;
        }
        if (this.CurerntChar == '.')
        {
            NextChar(); len++;
            while (IsDigit(this.CurerntChar))
            {
                NextChar(); len++;
            }
        }
        return float.Parse(_expr.Substring(start, len));
    }

    private float ScanFraction()
    {
        int start = _index - 2;
        int len = 1; // '.'
        while (IsDigit(this.CurerntChar))
        {
            NextChar(); len++;
        }
        return float.Parse(_expr.Substring(start, len));
    }

    private string ScanString()
    {
        char endChar = this.CurerntChar;
        NextChar();
        int start = _index - 1;
        int len = 0;
        while (this.CurerntChar != endChar)
        {
            if (!NextChar())
            {
                //                throw XPathException.Create(Res.Xp_UnclosedString);
            }
            len++;
        }
        NextChar();
        return _expr.Substring(start, len);
    }

    private string ScanName()
    {
        int start = _index - 1;
        int len = 0;

        for (;;)
        {
            if (_cChar == '\'' || _cChar == '"')
            {
                NextChar();
                len++;
            }
            else {
                break;
            }
        }
        return _expr.Substring(start, len);
    }
    #endregion

    #region Utility
    private static readonly char[] whitespaces = new[] { ' ', '\t', '\n', '\r' };
    private static bool IsWhiteSpace(char c)
    {
        return whitespaces.Contains(c);
    }
    private static bool IsDigit(char ch)
    {
        return InRange(ch, 0x30, 0x39);
    }
    private static bool InRange(int value, int start, int end)
    {
        return (uint)(value - start) <= (uint)(end - start);
    }
    #endregion

    public enum LexKind
    {
        Comma = ',',
        Slash = '/',
        At = '@',
        Dot = '.',
        LParens = '(',
        RParens = ')',
        LBracket = '[',
        RBracket = ']',
        Star = '*',
        Plus = '+',
        Minus = '-',
        Eq = '=',
        Lt = '<',
        Gt = '>',
        Bang = '!',
        Dollar = '$',
        Apos = '\'',
        Quote = '"',
        Union = '|',
        Ne = 'N',   // !=
        Le = 'L',   // <=
        Ge = 'G',   // >=
        And = 'A',   // &&
        Or = 'O',   // ||
        DotDot = 'D',   // ..
        SlashSlash = 'S',   // //
        Name = 'n',   // XML _Name
        String = 's',   // Quoted string constant
        Number = 'd',   // _Number constant
        Axe = 'a',   // Axe (like child::)
        Eof = 'E',
    };
}*/