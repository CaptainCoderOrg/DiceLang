using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    private static Parser<T> WithParenthesis<T>(Parser<T> toWrap) =>
        from leading in Parse.WhiteSpace.Many()
        from open in Parse.Char('(')
        from wrapped in toWrap
        from trailing in Parse.WhiteSpace.Many()
        from close in Parse.Char(')')
        select wrapped;
}