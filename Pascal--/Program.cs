using Lexer;

var sr = new StreamReader("D:/Pascal--/test.txt");
var lexer = new Lexer.Lexer(sr);

for (;;)
{
    var token = lexer.NextToken();
    if (token.TokenType == TokenType.EOF)
    {
        Console.WriteLine(new Token(' ',' ',TokenType.EOF,' ', ""));
        break;
    }
    Console.WriteLine(token);
    // if (token is EOF)
    // break
}