using System.Globalization;
namespace Lexer;
public class Lexer
{
    private StreamReader _reader;
    private string _buffer;
    private string _subbuffer;
    private int _number;
    private int _value;
    private int _simbol;
    private int _s_position;
    private int _string = 1;
    
    char Read()
    {
        var c = (char)_reader.Read();
        _buffer += c;
        _simbol += 1;
        return c;
    }
    bool EOF()
    {
        return _reader.EndOfStream;
    }
    public Lexer(StreamReader reader)
    {
        _reader = reader;
    }
    public Token NextToken()
    {
        _buffer = "";
        var c = (char)_reader.Peek();
        _s_position = 1 + _simbol;
        if (c is >= '0' and <= '9')
        {
            return ScanNumber();
        }
        if (c is '$' or '%' or '&')
        {
            Read();
            return ScanSistemNum();
        }
        {
            if ((c is >= 'a' and <= 'z') || (c is >= 'A' and <= 'Z') || (c is '_'))
            {
                return ScanId();
            }
            if (c == '\'')
            {
                return ScanString();
            }
            if (c == '/')
            {
                Read();
                if (_reader.Peek() == '/')
                {
                    
                    while (_reader.Peek() != '\n') 
                    {
                        Read();
                            if (EOF()) 
                                break; 
                    }
                }
                else
                {
                    while (_reader.Peek() != '\n')
                    {
                        Read();
                        if (EOF()) 
                            break; 
                    }
                    return new Token(_string,_s_position,TokenType.NONE, ' ', _buffer);
                }
            }
            if (c == '{') 
            {
                for(;;)
                {
                    Read();
                    if (_reader.Peek() == '}')
                    {
                        Read();
                        break;
                    }
                }
            }
            Read();
            switch (_buffer)
            {
                case "(":
                {
                    if (_reader.Peek() == '*')
                    {
                        Read();
                        for (;;)
                        {
                            Read();
                            if (_buffer.EndsWith('*') && (_reader.Peek() == ')'))
                            {
                                Read();
                                return NextToken();
                            }
                            if (EOF())
                            {
                                return new Token(_string,_s_position,TokenType.NONE, ' ', _buffer);
                            }
                        }
                    }
                    return new Token(_string,_s_position,TokenType.SEPARATORS, "LeftRoundBracket", _buffer);
                }
                    
                case ")": return new Token(_string,_s_position,TokenType.SEPARATORS, "RightRoundBracket", _buffer);
                case "[": return new Token(_string,_s_position,TokenType.SEPARATORS, "LeftSquareBracket", _buffer);
                case "]": return new Token(_string,_s_position,TokenType.SEPARATORS, "RightSquareBracket", _buffer);
                case ".":
                {
                    if (_reader.Peek() == '.')
                    {
                        Read();
                        switch (_buffer)
                        {
                            case "..": return new Token(_string,_s_position,TokenType.OPERATOR, "Range", _buffer);
                        }
                    }
                    return new Token(_string,_s_position,TokenType.SEPARATORS, "Dot", _buffer);
                }
                case ":":
                {
                    if (_reader.Peek() == '=')
                    {
                        Read();
                        switch (_buffer)
                        {
                            case ":=": return new Token(_string,_s_position,TokenType.OPERATOR, "Assign", _buffer);
                        }
                    }
                    return new Token(_string,_s_position,TokenType.SEPARATORS, "Сolon", _buffer);
                }
                case ";": return new Token(_string,_s_position,TokenType.SEPARATORS, "Semicolon", _buffer);
                case "=": return new Token(_string,_s_position,TokenType.OPERATOR, "Equal", _buffer);
                case "<":
                {
                    if ((_reader.Peek() == '<') || (_reader.Peek() == '>') || (_reader.Peek() == '='))
                    {
                        Read();
                        switch (_buffer)
                        {
                            case "<<": return new Token(_string,_s_position,TokenType.OPERATOR, "BitwiseShiftToTheLeft", _buffer);
                            case "<=": return new Token(_string,_s_position,TokenType.OPERATOR, "LessThanOrEqual", _buffer);
                            case "<>": return new Token(_string,_s_position,TokenType.OPERATOR, "NotEqual", _buffer); 
                        }
                    }
                    return new Token(_string,_s_position,TokenType.OPERATOR, "GreaterThan", _buffer);
                }
                case ">":
                { 
                    if ((_reader.Peek() == '>') || (_reader.Peek() == '='))
                    {
                        Read();
                        switch (_buffer)
                        {
                            case ">>": return new Token(_string,_s_position,TokenType.OPERATOR, "BitwiseShiftToTheRight", _buffer);
                            case ">=": return new Token(_string,_s_position,TokenType.OPERATOR, "GreaterOrEqual", _buffer);
                        }
                    }
                    return new Token(_string,_s_position,TokenType.OPERATOR, "LessThan", _buffer);
                }
                case "+":
                {
                    if (_reader.Peek() == '=')
                    {
                        Read();
                        switch (_buffer)
                        {
                            case "+=": return new Token(_string,_s_position,TokenType.OPERATOR, "AdditionAssign", _buffer);
                        }
                    }
                    return new Token(_string,_s_position,TokenType.OPERATOR, "Addition", _buffer);
                }
                case "-":
                {
                    if (_reader.Peek() == '=')
                    {
                        Read();
                        switch (_buffer)
                        {
                            case "-=": return new Token(_string,_s_position,TokenType.OPERATOR, "SubtractionAssign", _buffer);
                        }
                    }
                    return new Token(_string,_s_position,TokenType.OPERATOR, "Subtraction", _buffer);
                }
                case "*":
                {
                    if (_reader.Peek() == '=')
                    {
                        Read();
                        switch (_buffer)
                        {
                            case "*=": return new Token(_string,_s_position,TokenType.OPERATOR, "MultiplicationAssign", _buffer);
                        }
                    }
                    return new Token(_string,_s_position,TokenType.OPERATOR, "Multiplication", _buffer);
                }
                case "/":
                {
                    if (_reader.Peek() == '=')
                    {
                        Read();
                        switch (_buffer)
                        {
                            case "/=": return new Token(_string,_s_position,TokenType.OPERATOR, "DivisionAssign", _buffer);
                        }
                    }
                    return new Token(_string,_s_position,TokenType.OPERATOR, "Division", _buffer);
                }
            }

            if (c == '\n')
            {
                _simbol = 0;
                _string += 1;
                
            }
            
            {
                if (EOF())
                    return new Token(_string,_s_position,TokenType.EOF,' ', _buffer);
            }
        }
        return NextToken();
    }

    Token ScanNumber()
    {
        while ('0' <= _reader.Peek() && _reader.Peek() <= '9')
        {
            Read();
        }
        if (_reader.Peek() is '.')
        {
            Read();
            while ('0' <= _reader.Peek() && _reader.Peek() <= '9')
                Read();
            return new Token(_string,_s_position,TokenType.DOUBLE, _buffer, _buffer);
        }
        return new Token(_string,_s_position,TokenType.INTEGER, _buffer, _buffer);
    }

    Token ScanSistemNum()
    {
        if (_buffer == "%")
        {
            if (_reader.Peek() is >= '0' and <= '1')
            {
               while ('0' <= _reader.Peek() && _reader.Peek() <= '1')
               {
                   Read();
                   if (_reader.Peek() == '.')
                   {
                       Read();
                       while ('0' <= _reader.Peek() && _reader.Peek() <= '1')
                           Read();
                       return new Token(_string,_s_position,TokenType.DOUBLE, _buffer, _buffer);
                   }
               }
               _subbuffer = _buffer.Substring(1);
               _value = Convert.ToInt32(_subbuffer, 2);
               _subbuffer = Convert.ToString(_value);
               return new Token(_string,_s_position,TokenType.INTEGER, _buffer, _subbuffer);
            }
            return new Token(_string,_s_position,TokenType.NONE, _buffer, _buffer);
        }

        if (_buffer == "&")
        {
            if (_reader.Peek() is >= '0' and <= '7')
            {
                while ('0' <= _reader.Peek() && _reader.Peek() <= '7')
                {
                    Read();
                    if (_reader.Peek() == '.')
                    {
                        Read();
                        while ('0' <= _reader.Peek() && _reader.Peek() <= '7')
                            Read();
                        return new Token(_string,_s_position,TokenType.DOUBLE, _buffer, _buffer);
                    }
                } 
                _subbuffer = _buffer.Substring(1);
                return new Token(_string,_s_position,TokenType.INTEGER, _buffer, _subbuffer);
            }
            return new Token(_string,_s_position,TokenType.NONE, _buffer, _buffer);
        }
        if (_buffer == "$")
        {
            if (_reader.Peek() is (>= '0' and <= '9') or (>='a' and <= 'f') or (>='A' and <= 'F'))
            {
                while (('0' <= _reader.Peek() && _reader.Peek() <= '9') || ('a' <= _reader.Peek() && _reader.Peek() <= 'f') || ('A' <= _reader.Peek() && _reader.Peek() <= 'F'))
                {
                    Read();
                    if (_reader.Peek() == '.')
                    {
                        Read();
                        while (('0' <= _reader.Peek() && _reader.Peek() <= '9') || ('a' <= _reader.Peek() && _reader.Peek() <= 'f') || ('A' <= _reader.Peek() && _reader.Peek() <= 'F'))
                            Read();
                        return new Token(_string,_s_position,TokenType.DOUBLE, _buffer, _buffer);
                    }
                }
                _subbuffer = _buffer.Substring(1);
                _value = Convert.ToInt32(_subbuffer, 16);
                _subbuffer = Convert.ToString(_value);
                return new Token(_string,_s_position,TokenType.INTEGER, _buffer, _subbuffer);
            }
            return new Token(_string,_s_position,TokenType.NONE, _buffer, _buffer);
        } 
        return new Token(_string,_s_position,TokenType.NONE, _buffer, _buffer);
    }
    Token ScanId()
    {
        while (('a' <= _reader.Peek() && _reader.Peek() <= 'z') || ('1' <= _reader.Peek() && _reader.Peek() <= '9') ||
               '_' <= _reader.Peek() || ('A' <= _reader.Peek() && _reader.Peek() <= 'Z'))
        {
            Read();
        }
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        string lower = ti.ToLower(_buffer);
        if (Enum.IsDefined(typeof(Keywords), ti.ToTitleCase(lower)))
        {
            return new Token(_string,_s_position,TokenType.KEYWORD, _buffer, _buffer);
        }
        return new Token(_string,_s_position,TokenType.IDENTIFIER, _buffer, _buffer);
        
    }

    Token ScanString()
    {
        for(;;)
        {
            Read();
            if (_reader.Peek() == '\n')
            {
                while (_reader.Peek() != '\'')
                    Read();
                Read();
                return new Token(_string,_s_position,TokenType.NONE, ' ', _buffer);
            }
            if (_reader.Peek() == '\'')
            {
                Read();
                break;
            }
        }
        return new Token(_string,_s_position,TokenType.STRING, _buffer, _buffer);
    }

    public enum Keywords
    {
        And,
        Array,
        Asm,
        Begin,
        Break,
        Case,
        Const,
        Constructor,
        Continue,
        Destructor,
        Div,
        Do,
        Downto,
        Else,
        End,
        False,
        File,
        For,
        Function,
        Goto,
        If,
        Implementation,
        In,
        Inline,
        Interface,
        Label,
        Mod,
        Nil,
        Not,
        Object,
        Of,
        Operator,
        Or,
        Packed,
        Procedure,
        Program,
        Record,
        Repeat,
        Set,
        Shl,
        Shr,
        String,
        Then,
        To,
        True,
        Type,
        Unit,
        Until,
        Uses,
        Var,
        While,
        With,
        Xor,
        As,
        Class,
        Constref,
        Dispose,
        Except,
        Exit,
        Exports,
        Finalization,
        Finally,
        Inherited,
        Initialization,
        Is,
        Library,
        New,
        On,
        Out,
        Property,
        Raise,
        Self,
        Threadvar,
        Try,
        Absolute,
        Abstract,
        Alias,
        Assembler,
        Cdecl,
        Cppdecl,
        Default,
        Export,
        External,
        Forward,
        Generic,
        Index,
        Local,
        Name,
        Nostackframe,
        Oldfpccall,
        Override,
        Pascal,
        Private,
        Protected,
        Public,
        Published,
        Read,
        Register,
        Reintroduce,
        Safecall,
        Softfloat,
        Specialize,
        Stdcall,
        Virtual,
        Write
    }
}  