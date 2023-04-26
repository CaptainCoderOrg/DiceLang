using Sprache;
namespace CaptainCoder.DiceLang;

// OrExpr: AndExpr (|| AndExpr)*
// AndExpr: FactorExpr (&& FactorExpr)*
// FactorExpr: (OrExpr) | NotExpr | BoolValue
// NotExpr: ! OrExpr
public static partial class Parsers
{
    public static Parser<IExpression> NotExpr => 
        from expr in Parse.Char('!').Token().Then((_) => BooleanFactorExpression.Token())
        select new NotExpression(expr);
    
    public static Parser<IExpression> OrExpression => 
        from leftSide in AndExpression.Token()
        from remainingExpr in RightSideOrExpression(leftSide).Optional()
        select remainingExpr.GetOrElse(leftSide);
    
    private static Parser<IExpression> RightSideOrExpression(IExpression leftSide) =>
        from factor in Tokenize("||", AndExpression)
        from optionalMany in Parse.Optional(RightSideOrExpression(factor))
        select new OrExpression(leftSide, optionalMany.GetOrElse(factor));
        
    public static Parser<IExpression> AndExpression => 
        from leftSide in Parse.WhiteSpace.Many().Then((_) => BooleanFactorExpression)
        from remainingExpr in RightSideAndExpr(leftSide).Optional()
        select remainingExpr.GetOrElse(leftSide);

    private static Parser<IExpression> RightSideAndExpr(IExpression leftSide)
    {
        return
        from factor in Tokenize("&&", BooleanFactorExpression)
        from optionalMany in Parse.Optional(RightSideAndExpr(factor))
        select new AndExpression(leftSide, optionalMany.GetOrElse(factor));
    }


    public static Parser<IExpression> BooleanFactorExpression =>
        WithParenthesis(OrExpression)
        .Or(RelationalExpr)
        .Or(NotExpr)
        .Or(BoolValueExpression);

    public static Parser<IExpression> ConditionalExpression => OrExpression; 
}