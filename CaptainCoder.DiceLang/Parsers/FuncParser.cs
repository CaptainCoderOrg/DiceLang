using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{

    public static Parser<IExpression> FuncExpr => 
        from fun in Parse.String("fun").Token()
        from openParen in Parse.Char('(').Token()
        from paramId in IdentifierString.Token()
        from closeParen in Parse.Char(')').Token()
        from arrow in Parse.String("=>").Token()
        from expr in DiceLangExpressionInner
        select new FuncValue(paramId, expr);

    // (fun (x) => fun (y) => x + y) (5) (3)
    public static Parser<IExpression> ApplyFuncExpr => 
        from funcExpr in ApplyFuncFactor
        from argExpr in ArgumentExpr
        from optionalArgs in ArgumentExpr.Many()
        select AggregateArguments(new ApplyFuncExpression(funcExpr, argExpr), optionalArgs.ToList());

    // f (5) (3) (1)
    private static ApplyFuncExpression AggregateArguments(ApplyFuncExpression funcExpr, List<IExpression> args)
    {
        foreach (IExpression arg in args)
        {
            funcExpr = new ApplyFuncExpression(funcExpr, arg);
        }
        return funcExpr;
    }

    private static Parser<IExpression> ArgumentExpr => WithParenthesis(DiceLangExpressionInner).Token();

    public static Parser<IExpression> ApplyFuncFactor => 
        WithParenthesis(FuncExpr.Token()).Or(IdentifierExpr);
}