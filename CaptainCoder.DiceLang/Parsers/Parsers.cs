using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{

    public static Parser<IExpression> DiceLangExpressionInner =>
            ApplyFuncExpr
            .Or(ConditionalExpression)
            .Or(LetExpr)
            .Or(IfElseExpr)
            .Or(ArithmeticExpression)
            .Or(FuncExpr).Token();

    public static Parser<IExpression> DiceLangExpression => DiceLangExpressionInner.End();
}