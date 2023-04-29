using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{

    public static Parser<IExpression> DiceLangExpressionInner =>
            ConditionalExpression
            .Or(ArithmeticExpression)
            .Or(ApplyFuncExpr)
            .Or(LetExpr)
            .Or(IfElseExpr)            
            .Or(FuncExpr).Token();

    public static Parser<IExpression> DiceLangExpression => DiceLangExpressionInner.End();
}