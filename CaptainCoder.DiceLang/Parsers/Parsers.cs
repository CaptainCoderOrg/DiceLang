using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{

    private static Parser<IExpression> DiceLangExpressionInner =>
            ConditionalExpression
            .Or(LetExpr)
            .Or(IfElseExpr)
            .Or(ArithmeticExpression).Token();

    public static Parser<IExpression> DiceLangExpression => DiceLangExpressionInner.End();
}