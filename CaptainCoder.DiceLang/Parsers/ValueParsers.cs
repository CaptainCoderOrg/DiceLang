using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    public static Parser<CharValue> CharValue =>
        from leadingSpace in Parse.WhiteSpace.Many()
        from singleQuote in Parse.Char('\'')
        from character in Parse.AnyChar
        from endQuote in Parse.Char('\'')
        select new CharValue(character);

    public static Parser<IExpression> BoolValue =>
        from value in Parse.String("true").Or(Parse.String("false")).Token()
        select new BoolValue(string.Join("", value) == "true");

    public static Parser<IntValue> IntValue =>
        from value in Parse.Digit.AtLeastOnce().Token()
        select new IntValue(int.Parse(string.Join("", value)));

    public static Parser<DoubleValue> DoubleValue =>
        from left in Parse.Digit.AtLeastOnce().Token()
        from dot in Parse.Char('.')
        from right in Parse.Digit.AtLeastOnce().Token()
        select new DoubleValue(double.Parse(string.Join("", left) + "." + string.Join("", right)));

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
    
    public static Parser<string> IdentifierString =>
        from id in Parse.Letter.AtLeastOnce().Token()
        select string.Join("", id);

    public static Parser<IdentifierValue> IdentifierExpr =>
        from _ in NotKeywords()
        from id in Parse.Letter.AtLeastOnce().Token()
        select new IdentifierValue(string.Join("", id));

    public static Parser<IExpression> BoolValueExpression =>
    BoolIdentifier.Or(BoolValue);

    public static readonly string[] Operators = new string[]
        {"+", "-", "*", "/", "<", "<=", ">=", "!=", ">", "==", "("};
    public static Parser<object> NotOperator() 
    {

        Parser<object> parser = null;
        foreach (string @operator in Operators)
        {
            if (parser == null)
            {
                parser = Parse.String(@operator).Token();
            }
            else
            {
                parser = parser.Or(Parse.String(@operator).Token());
            }            
        }
        return parser.Not();
    }
    
        
        // ("[+|-|*|/|<|>|==]").Token();

    public static Parser<IExpression> BoolIdentifier =>
        from id in IdentifierExpr.Token()
        from _ in NotOperator()
        select id;

    public static Parser<IExpression> NumericValueExpression =>
    (DiceGroupExpression as Parser<IExpression>)
    .Or(DoubleValue)
    .Or(IntValue)
    .Or(CharValue)
    .Or(IdentifierExpr);

}