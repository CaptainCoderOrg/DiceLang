using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    // TODO: ArithmeticExpression is incorrect for true/false branch. Should be any expression.
    public static Parser<IExpression> IfElseExpr =>
        from condExpr in Tokenize("if", RelationalExpr.Or(BoolValue))
        from trueExpr in Tokenize("then", DiceLangExpressionInner)
        from falseExpr in Tokenize("else", DiceLangExpressionInner)
        select new IfElseExpression(condExpr, trueExpr, falseExpr);


    //if5<3then0else1

    // public static Parser<IExpression> IfElseExprExample =>
    //     from leading in Parse.WhiteSpace.Many() // [w]*
    //     from ifToken in Parse.String("if") // if
    //     from whiteSpace in Parse.WhiteSpace.Many() // [w]+
    //     from relExpr in RelationalExpr.Or(BoolValue) // 
}