using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{

    public static Parser<IExpression> FuncExpr =>
    from fun in Parse.String("fun").Token()
        from openParen in Parse.Char('(').Token()
        from firstParam in IdentifierString.Token()
        // Parse , more params
        from additionalParams in AdditionalParam.Many()
        from closeParen in Parse.Char(')').Token()
        from arrow in Parse.String("=>").Token()
        from expr in DiceLangExpressionInner
        select BuildMultiParamFunc(firstParam, additionalParams.ToList(), expr);

    // f (x, y) => x + y
    // new FuncValue("x", new FuncValue("y", x + y));
    // f (x, y, z) => x + y + z
    // new FuncValue("x", new FuncValue("y", new FuncValue("z", x + y + z)))
    public static FuncValue BuildMultiParamFunc(string firstParmeter, List<string> remainingParameters, IExpression body)
    {
        if (remainingParameters.Count == 0) { return new FuncValue(firstParmeter, body); }
        string head = remainingParameters[^1]; // remainingParameters[remainingParameters.Count - 1]
        remainingParameters.RemoveAt(remainingParameters.Count-1);
        return BuildMultiParamFunc(firstParmeter, remainingParameters, new FuncValue(head, body));
    }

    public static Parser<string> AdditionalParam => Tokenize(",", IdentifierString.Token());
        
    // public static Parser<IExpression> FuncExpr => 
    //     from fun in Parse.String("fun").Token()
    //     from openParen in Parse.Char('(').Token()
    //     from paramId in IdentifierString.Token()
    //     from closeParen in Parse.Char(')').Token()
    //     from arrow in Parse.String("=>").Token()
    //     from expr in DiceLangExpressionInner
    //     select new FuncValue(paramId, expr);

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

    private static Parser<IExpression> ArgumentExpr => WithParenthesis(DiceLangExpressionInner);

    public static Parser<IExpression> ApplyFuncFactor => 
        WithParenthesis(FuncExpr.Token()).Or(IdentifierExpr);
}