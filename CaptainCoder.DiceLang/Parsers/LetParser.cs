using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    // TODO: ArithmeticExpression is incorrect for true/false branch. Should be any expression.
    // let id = value in body
    public static Parser<IExpression> LetExpr =>
        from identifier in Tokenize("let", IdentifierString)
        from valueExpr in Tokenize("=", DiceLangExpressionInner)
        from bodyExpr in Tokenize("in", DiceLangExpressionInner)
        select new LetExpression(identifier, valueExpr, bodyExpr);

    
}