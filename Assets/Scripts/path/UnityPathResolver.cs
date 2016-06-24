using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnityPathResolver
{
    public enum PathPartType
    {
        Find, Component, Acceessor
    }
    private List<KeyValuePair<PathPartType, string>> _pathParts = new List<KeyValuePair<PathPartType, string>>();
    public KeyValuePair<PathPartType, string>[] PathParts { get { return _pathParts.ToArray(); } }

    private char _cChar;
    private int _index;
    private readonly string _expr;
    private readonly List<Func<object, object>> _actions = new List<Func<object, object>>();

    public Func<object, object>[] actions { get { return _actions.ToArray(); } }
    public UnityPathResolver(string path)
    {
        _expr = path.Trim();
        NextChar();
        while (NextLex()) ;
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
    private char PeekNextChar()
    {
        return _index < _expr.Length ? _expr[_index] : '\0';
    }


    public bool NextLex()
    {
        SkipSpace();
        string s;
        switch (_cChar)
        {
            case '\0':
                return false;
            case ']':
            case ')':
                throw new Exception("Unexpected symbol" + _cChar);
            case '(':
                s = ReadString();
                _pathParts.Add(new KeyValuePair<PathPartType, string>(PathPartType.Find, s));
                _actions.Add((o) => UnityReflectionAccessor.Find(o, s));
                break;
            case '[':
                s = ReadClass();
                _pathParts.Add(new KeyValuePair<PathPartType, string>(PathPartType.Component, s));
                _actions.Add((o) => UnityReflectionAccessor.FindComponent(o, s));
                break;
            default:
                s = ReadName();
                _pathParts.Add(new KeyValuePair<PathPartType, string>(PathPartType.Acceessor, s));
                _actions.Add((o) => UnityReflectionAccessor.Access(o, s));
                break;
        }
        return true;
    }
    /// <summary>
    /// accessor name
    /// should start with letter
    /// </summary>
    private string ReadName()
    {
        if (_cChar == '.')
            NextChar();
        SkipSpace();

        int start = _index - 1;
        int len = 0;
        while (char.IsLetterOrDigit(_cChar))
        {
            len++;
            if (!NextChar())
                break;
        }
        NextChar();
        return _expr.Substring(start, len);
    }

    /// <summary>
    /// reading [Class] for GameObject.GetComponent&lt;Class&gt;();
    /// </summary>
    /// <returns></returns>
    private string ReadClass()
    {
        char endChar = ']';
        int start = _index;
        int len = 0;
        while (_cChar != endChar)
        {
            if (!NextChar())
                throw new Exception("Unclosed Bracket");
            len++;
        }
        NextChar();
        string className = _expr.Substring(start, len - 1).Trim();
        if (!char.IsLetter(className[0]) && className[0] != '_')
            throw new Exception("Class name should start with letter or underscore");
        if (className.FirstOrDefault(x => !char.IsLetterOrDigit(x) && x != '_') != '\0')
            throw new Exception("Class name can contain only letters, digits and underscore");
        return className;
    }

    /// <summary>
    /// reading (path) for Transform.find(path);
    /// use (( = (
    /// and )) = )
    /// for escaping
    /// and be sure not to use slashes in names - it will confuse unity
    /// </summary>
    /// <returns></returns>
    private string ReadString()
    {
        int start = _index;
        bool isEscapeParens = false;

        while ((isEscapeParens = IsEscapeParens(_cChar, PeekNextChar())) || _cChar != ')')
        {
            if (isEscapeParens)
                NextChar();
            if (!NextChar())
                throw new Exception("Unclosed Parens");
        }

        NextChar();
        return _expr.Substring(start, _index - start - 2).Replace("((", "(").Replace("))", ")");
    }

    private static bool IsEscapeParens(char c1, char c2)
    {
        if (c1 == c2 && c1 == '(') return true;
        if (c1 == c2 && c1 == ')') return true;
        return false;
    }

    void SkipSpace()
    {
        while (char.IsWhiteSpace(this._cChar) && NextChar()) ;
    }

    public static KeyValuePair<UnityPathResolver.PathPartType, string>[] GetPathParts(string path)
    {
        return new UnityPathResolver(path).PathParts;
    }

    public static object Resolve(object o, string path)
    {
        return UnityPathResolver.Resolve(o, new UnityPathResolver(path).PathParts);
    }
    public static object Resolve(object o, KeyValuePair<UnityPathResolver.PathPartType, string>[] pathParts)
    {
        var cObj = o;
        foreach (var part in pathParts)
        {
            switch (part.Key)
            {
                case PathPartType.Find:
                    cObj = UnityReflectionAccessor.Find(cObj, part.Value);
                    break;
                case PathPartType.Component:
                    cObj = UnityReflectionAccessor.FindComponent(cObj, part.Value);
                    break;
                case PathPartType.Acceessor:
                    cObj = UnityReflectionAccessor.Access(cObj, part.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return cObj;
    }
}