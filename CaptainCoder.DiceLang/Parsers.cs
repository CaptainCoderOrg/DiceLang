using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{

    private static Parser<IExpression> _diceLangExpression =>
        from leading in Parse.WhiteSpace.Many()
        from expression in BoolValue.Or(IfElseExpr).Or(ConditionalExpr).Or(ArithmeticExpression)
        from training in Parse.WhiteSpace.Many()
        select expression;

    public static Parser<IExpression> DiceLangExpression => _diceLangExpression.End();
}