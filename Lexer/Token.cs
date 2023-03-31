namespace Lexer
{
    public enum TokenType
    {
        NONE,
        DOUBLE,
        INTEGER,
        STRING,
        IDENTIFIER,
        KEYWORD,
        OPERATOR,
        EOF,
        SEPARATORS
    }

    public class Token
    {
        public Token(int sstring,int column,TokenType tokenType,object value, string source)
        {
            String = sstring;
            Column = column;
            TokenType = tokenType;
            Value = value;
            Source = source;
        }
        public int String { get; }
        public int Column { get; }
        public TokenType TokenType { get; }
        public object Value { get; }
        public string Source { get; }
        
        
        public override string ToString()
        {
            return $"{String}\t{Column}\t{TokenType}\t\t{Value}\t{Source}";
        }
    }
}