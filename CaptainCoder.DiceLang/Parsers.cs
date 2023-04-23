using Sprache;
namespace CaptainCoder.DiceLang;

public static class Parsers
{

    public static Parser<IEnumerable<char>> ParseRightSideOperator(char symbol)
    {
        return
        from whiteSpace in Parse.WhiteSpace.Many()
        from plusSymbol in Parse.Char(symbol)
        from whiteSpace_ in Parse.WhiteSpace.Many()
        select whiteSpace.Concat(new char[]{symbol}).Concat(whiteSpace_);
    }    

    public static Parser<IntValue> IntValue =>
        from leading in Parse.WhiteSpace.Many()
        from value in Parse.Digit.AtLeastOnce()
        select new IntValue(int.Parse(string.Join("", value)));

    public static Parser<DiceGroupExpression> DiceGroupExpression =>
        from leading in Parse.WhiteSpace.Many()
        from diceCount in Parse.Digit.AtLeastOnce()
        from dChar in Parse.Char('d')
        from sideCount in Parse.Digit.AtLeastOnce()
        select FromParseResult(diceCount, sideCount);

    private static DiceGroupExpression FromParseResult(IEnumerable<char> diceCountChars, IEnumerable<char> sideCountChars)
    {
        int diceCount = int.Parse(string.Join("", diceCountChars));
        if (diceCount < 1) { throw new ParseException($"Could not parse {nameof(DiceGroupExpression)} with dice count of {diceCount}."); }
        int sideCount = int.Parse(string.Join("", sideCountChars));
        if (sideCount < 1) { throw new ParseException($"Could not parse {nameof(DiceGroupExpression)} with side count of {sideCount}."); }
        return DiceLang.DiceGroupExpression.WithDefaultSource(diceCount, sideCount);
    }

    public static Parser<IExpression> NumericExpression =>
        (DiceGroupExpression as Parser<IExpression>)
        .Or(IntValue)
    ;

    // AddSubExpr ::= NumExpr [[+|-] AddSubExpr]

    public static Parser<IExpression> AddSubExpression =>
        from leading in Parse.WhiteSpace.Many()
        from leftSide in MulDivExpression
        from remainingExpr in Parse.Optional(RightSideAddition(leftSide).Or(RightSideSubtraction(leftSide)))
        select remainingExpr.GetOrElse(leftSide);

    private static Parser<IExpression> RightSideAddSubExpression(IExpression leftSide, char symbol, Func<IExpression, IExpression, IExpression> constructor)
    {
        return
        from op in ParseRightSideOperator(symbol)
        from factor in MulDivExpression 
        from optionalMany in Parse.Optional(RightSideAddition(factor).Or(RightSideSubtraction(factor)))
        select constructor(leftSide, optionalMany.GetOrElse(factor));
    }

    public static Parser<IExpression> RightSideAddition(IExpression leftSide) =>
        RightSideAddSubExpression(leftSide, '+', (left, right) => new AdditionExpression(left, right));

    public static Parser<IExpression> RightSideSubtraction(IExpression leftSide) =>
        RightSideAddSubExpression(leftSide, '-', (left, right) => new SubtractionExpression(left, right));

    private static Parser<IExpression> RightSideFactorOperator(IExpression leftSide, char symbol, Func<IExpression, IExpression, IExpression> constructor)
    {
        return
        from op in ParseRightSideOperator(symbol)
        from factor in FactorExpression 
        from optionalMany in Parse.Optional(RightSideMultiplication(constructor(leftSide, factor)).Or(RightSideDivision(constructor(leftSide, factor)))) // 
        select optionalMany.GetOrElse(constructor(leftSide, factor));
    }

    public static Parser<IExpression> RightSideMultiplication(IExpression leftSide) =>
        RightSideFactorOperator(leftSide, '*', (IExpression a, IExpression b) => new MultiplicationExpression(a, b));

    public static Parser<IExpression> RightSideDivision(IExpression leftSide) =>
        RightSideFactorOperator(leftSide, '/', (IExpression a, IExpression b) => new DivisionExpression(a, b));

    // AddSubExpr: MulDivExpr ([+|-] MulDivExpr)* 
    // MulDivExpr: FactorExpr ([*|/] FactorExpr)*
    // FactorExpr: (AddSubExpr) | Value
    public static Parser<IExpression> MulDivExpression =>
        from leading in Parse.WhiteSpace.Many()
        from leftSide in FactorExpression
        from remainingExpr in Parse.Optional(RightSideMultiplication(leftSide).Or<IExpression>(RightSideDivision(leftSide)))
        select remainingExpr.GetOrElse(leftSide);

    public static Parser<IExpression> FactorExpression =>
        WithParenthesis(AddSubExpression).Or(NumericExpression);

    public static Parser<IExpression> ArithmeticExpression => AddSubExpression;

    public static Parser<T> WithParenthesis<T>(Parser<T> toWrap) =>
        from leading in Parse.WhiteSpace.Many()
        from open in Parse.Char('(')
        from wrapped in toWrap
        from trailing in Parse.WhiteSpace.Many()
        from close in Parse.Char(')')
        select wrapped;

}