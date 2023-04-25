using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    // AddSubExpr ::= NumExpr [[+|-] AddSubExpr]
    public static Parser<IExpression> AddSubExpression =>
        from leftSide in Tokenize(MulDivExpression)
        from remainingExpr in Parse.Optional(RightSideAddition(leftSide).Or(RightSideSubtraction(leftSide)))
        select remainingExpr.GetOrElse(leftSide);

    private static Parser<IExpression> RightSideAddSubExpression(IExpression leftSide, string symbol, Func<IExpression, IExpression, IExpression> constructor)
    {
        return
        from factor in Tokenize(symbol, MulDivExpression)
        from optionalMany in Parse.Optional(RightSideAddition(factor).Or(RightSideSubtraction(factor)))
        select constructor(leftSide, optionalMany.GetOrElse(factor));
    }

    public static Parser<IExpression> RightSideAddition(IExpression leftSide) =>
        RightSideAddSubExpression(leftSide, "+", (left, right) => new AdditionExpression(left, right));

    public static Parser<IExpression> RightSideSubtraction(IExpression leftSide) =>
        RightSideAddSubExpression(leftSide, "-", (left, right) => new SubtractionExpression(left, right));

    private static Parser<IExpression> RightSideFactorOperator(IExpression leftSide, string symbol, Func<IExpression, IExpression, IExpression> constructor)
    {
        return
        from factor in Tokenize(symbol, ArithmeticFactorExpression)
        from optionalMany in Parse.Optional(RightSideMultiplication(constructor(leftSide, factor)).Or(RightSideDivision(constructor(leftSide, factor)))) //
        select optionalMany.GetOrElse(constructor(leftSide, factor));
    }

    public static Parser<IExpression> RightSideMultiplication(IExpression leftSide) =>
        RightSideFactorOperator(leftSide, "*", (IExpression a, IExpression b) => new MultiplicationExpression(a, b));

    public static Parser<IExpression> RightSideDivision(IExpression leftSide) =>
        RightSideFactorOperator(leftSide, "/", (IExpression a, IExpression b) => new DivisionExpression(a, b));

    // AddSubExpr: MulDivExpr ([+|-] MulDivExpr)*
    // MulDivExpr: FactorExpr ([*|/] FactorExpr)*
    // FactorExpr: (AddSubExpr) | Value
    public static Parser<IExpression> MulDivExpression =>
        from leftSide in Parse.WhiteSpace.Many().Then((_) => ArithmeticFactorExpression)
        from remainingExpr in Parse.Optional(RightSideMultiplication(leftSide).Or(RightSideDivision(leftSide)))
        select remainingExpr.GetOrElse(leftSide);

    public static Parser<IExpression> ArithmeticFactorExpression =>
        WithParenthesis(AddSubExpression).Or(NumericExpression);

    public static Parser<IExpression> ArithmeticExpression => AddSubExpression;
}