using Sprache;
namespace CaptainCoder.DiceLang;

public static class Parsers
{

    public static Parser<IntValue> IntValue { get; } =
        from leading in Parse.WhiteSpace.Many()
        from value in Parse.Digit.AtLeastOnce()
        select new IntValue(int.Parse(string.Join("", value)));

    public static Parser<DiceGroupExpression> DiceGroupExpression { get; } =
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

    public static Parser<Expression> NumericExpression { get; } =
        (DiceGroupExpression as Parser<Expression>)
        .Or(IntValue)
    ;

    // AdditionExpr ::= NumExpr [+ AdditionExpr]

    private static Parser<AdditionExpression> OptionalAdditionExpression { get; }  =
        from leading in Parse.WhiteSpace.Many()
        from leftSide in NumericExpression
        from remainingExpr in RightSideOfAdditionExpression(leftSide)
        select remainingExpr;

    private static Parser<AdditionExpression> RightSideOfAdditionExpression(Expression leftSide)
    {
        return
        from whiteSpace in Parse.WhiteSpace.Many()
        from plusSymbol in Parse.Char('+')
        from whiteSpace_ in Parse.WhiteSpace.Many()
        from remainingExpr in AdditionExpression 
        select new AdditionExpression(leftSide, remainingExpr);
    }

    public static Parser<Expression> AdditionExpression { get; } =
        OptionalAdditionExpression.Or(NumericExpression);


}