using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    public static Parser<IExpression> ConditionalExpr 
    {
        get
        {
            return WithParenthesis(ConditionalExprInner).Or(ConditionalExprInner);
        }
    }
        
    private static Parser<IExpression> ConditionalExprInner =>
        GreaterThanExpression.Or(LessThanExpression).Or(EqualityExpression);
    private static Parser<IExpression> ConditionalExprHelper(string symbol, Func<IExpression, IExpression, IExpression> constructor)
    {
        return
        from leading in Parse.WhiteSpace.Many()
        from leftExpr in ArithmeticExpression
        from after in Parse.WhiteSpace.Many()
        from gtSymbol in Parse.String(symbol)
        from after_ in Parse.WhiteSpace.Many()
        from rightExpr in ArithmeticExpression
        select constructor(leftExpr, rightExpr);
    }
    public static Parser<IExpression> GreaterThanExpression =>
        ConditionalExprHelper(">", (left, right) => new GreaterThanExpression(left, right));

    public static Parser<IExpression> LessThanExpression =>
        ConditionalExprHelper("<", (left, right) => new LessThanExpression(left, right));

    public static Parser<IExpression> EqualityExpression =>
        ConditionalExprHelper("==", (left, right) => new EqualityExpression(left, right));
}