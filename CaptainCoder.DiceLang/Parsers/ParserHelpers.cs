using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    private static Parser<T> WithParenthesis<T>(Parser<T> toWrap) => Tokenize("(", toWrap, ")");

    private static Parser<T> Tokenize<T>(Parser<T> toParse) => toParse.Token();

    private static Parser<T> Tokenize<T>(string prefix, Parser<T> toParse) =>
        Tokenize(Parse.String(prefix)).Then((_) => toParse);

    private static Parser<T> Tokenize<T>(string prefix, Parser<T> toParse, string postfix) =>
        from token in Parse.String(prefix).Token().Then((_) => toParse)
        from symbolAfter in Parse.String(postfix).Token()
        select token;
}