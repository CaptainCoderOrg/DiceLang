using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    public static Parser<IExpression> BoolValue =>
        from value in Parse.String("true").Or(Parse.String("false")).Token()
        select new BoolValue(string.Join("", value) == "true");

    public static Parser<IntValue> IntValue =>
        from value in Parse.Digit.AtLeastOnce().Token()
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

    private static readonly string[] ReservedKeywords =
    {
        "let", "if", "then", "else", "fun", "true", "false"
    };

    public static Parser<object> NotKeywords()
    {

        Parser<object> parser = null;
        foreach (string keyword in ReservedKeywords)
        {
            if (parser == null)
            {
                parser = Parse.String(keyword).Token();
            }
            else
            {
                parser = parser.Or(Parse.String(keyword).Token());
            }            
        }
        return parser.Not();
    }
        
    // ReservedKeywords.Aggregate(Parse.String("foo").Token(), (string left, string right) => Parse.String(left).Token());
    
    public static Parser<string> IdentifierString =>
        from id in Parse.Letter.AtLeastOnce().Token()
        select string.Join("", id);

    public static Parser<IdentifierValue> IdentifierExpr =>
        from _ in NotKeywords()
        from id in Parse.Letter.AtLeastOnce().Token()
        select new IdentifierValue(string.Join("", id));

    public static Parser<IExpression> BoolValueExpression =>
    BoolIdentifier.Or(BoolValue);

    public static Parser<object> Operator =>
        Parse.RegexMatch("[+|-|*|/|<|>|==]").Token();

    public static Parser<IExpression> BoolIdentifier =>
        from id in IdentifierExpr.Token()
        from _ in Operator.Not()
        select id;

    public static Parser<IExpression> NumericValueExpression =>
    (DiceGroupExpression as Parser<IExpression>)
    .Or(IntValue)
    .Or(IdentifierExpr);

}