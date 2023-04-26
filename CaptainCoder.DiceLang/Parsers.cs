using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{

    private static Parser<IExpression> DiceLangExpressionInner =>
        from leading in Parse.WhiteSpace.Many()
        from expression in BooleanExpression.Or(IfElseExpr).Or(ArithmeticExpression).Or(NotExpr)
        from training in Parse.WhiteSpace.Many()
        select expression;

    public static Parser<IExpression> DiceLangExpression => DiceLangExpressionInner.End();
}