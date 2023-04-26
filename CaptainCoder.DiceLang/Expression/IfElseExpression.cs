using CaptainCoder.Core;
namespace CaptainCoder.DiceLang;


public record IfElseExpression(IExpression Condition, IExpression TrueExpr, IExpression FalseExpr) : IExpression
{
    public IValue Evaluate()
    {
        IValue conditionResult = Condition.Evaluate();
        return conditionResult.ToBool() ? TrueExpr.Evaluate() : FalseExpr.Evaluate();
    }

    public IExpression Substitute(string label, IExpression toSub)
    {
        return new IfElseExpression(
            Condition.Substitute(label, toSub),
            TrueExpr.Substitute(label, toSub),
            FalseExpr.Substitute(label, toSub)
        );
    }
}