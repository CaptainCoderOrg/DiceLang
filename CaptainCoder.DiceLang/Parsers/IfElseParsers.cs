using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    // TODO: ArithmeticExpression is incorrect for true/false branch. Should be any expression.
    public static Parser<IExpression> IfElseExpr =>
        from condExpr in Tokenize("if", ConditionalExpr.Or(BoolValue))
        from trueExpr in Tokenize("then", _diceLangExpression)
        from falseExpr in Tokenize("else", _diceLangExpression)
        select new IfElseExpression(condExpr, trueExpr, falseExpr);
}