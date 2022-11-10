namespace Lexer
{
    public struct Position
    {
        public int String { get; set; }
        public int Column { get; set; }

        public Position(int @string, int column)
        {
            String = @string;
            Column = column;
        }

        public override string ToString()
        {
            return $"{String}\t{Column}";
        }
    }
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
        public Token(TokenType tokenType,object value, string source)
        {
            TokenType = tokenType;
            Value = value;
            Source = source;
        }
        public Position Position { get; private set; }
        public TokenType TokenType { get; }
        public object Value { get; }
        public string Source { get; }
        

        public Token AddPosition(Position position)
        {
            Position = position;
            return this;
        }
        
        public override string ToString()
        {
            return $"{Position}\t{TokenType}\t\t{Value}\t{Source}";
        }
    }
}