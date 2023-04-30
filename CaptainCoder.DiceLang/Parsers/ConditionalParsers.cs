using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    public static Parser<IExpression> RelationalExpr =>
        GreaterThanExpression
        .Or(LessThanExpression)
        .Or(GreaterThanOrEqualToExpression)
        .Or(LessThanOrEqualToExpression)
        .Or(EqualityExpression)
        .Or(NotEqualityExpression);
    private static Parser<IExpression> ConditionalExprHelper(string symbol, Func<IExpression, IExpression, IExpression> constructor)
    {
        return
        from leftExpr in ArithmeticExpression.Token()
        from _ in Parse.String(symbol).Token()
        from rightExpr in ArithmeticExpression.Token()
        select constructor(leftExpr, rightExpr);
    }
    public static Parser<IExpression> GreaterThanExpression =>
        ConditionalExprHelper(">", (left, right) => new GreaterThanExpression(left, right));

    public static Parser<IExpression> GreaterThanOrEqualToExpression =>
        ConditionalExprHelper(">=", (left, right) => 
            new OrExpression(new GreaterThanExpression(left, right), new EqualityExpression(left, right)));

    public static Parser<IExpression> LessThanExpression =>
        ConditionalExprHelper("<", (left, right) => new LessThanExpression(left, right));

    public static Parser<IExpression> LessThanOrEqualToExpression =>
        ConditionalExprHelper("<=", (left, right) => 
            new OrExpression(new LessThanExpression(left, right), new EqualityExpression(left, right)));

    public static Parser<IExpression> EqualityExpression =>
        ConditionalExprHelper("==", (left, right) => new EqualityExpression(left, right));

    public static Parser<IExpression> NotEqualityExpression =>
        ConditionalExprHelper("!=", (left, right) => 
            new NotExpression(new EqualityExpression(left, right)));
}