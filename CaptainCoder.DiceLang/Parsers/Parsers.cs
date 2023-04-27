using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{

    private static Parser<IExpression> DiceLangExpressionInner =>
            ApplyFuncExpr
            .Or(ConditionalExpression)
            .Or(LetExpr)
            .Or(IfElseExpr)
            .Or(FuncExpr)
            .Or(ArithmeticExpression).Token();

    public static Parser<IExpression> DiceLangExpression => DiceLangExpressionInner.End();
}