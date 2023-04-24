using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    public static Parser<BoolValue> BoolValue =>
        from leading in Parse.WhiteSpace.Many()
        from value in Parse.String("true").Or(Parse.String("false"))
        select new BoolValue(string.Join("", value) == "true");

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
}