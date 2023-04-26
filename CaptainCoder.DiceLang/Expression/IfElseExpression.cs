using CaptainCoder.Core;
namespace CaptainCoder.DiceLang;


public record IfElseExpression(IExpression Condition, IExpression TrueExpr, IExpression FalseExpr) : IExpression
{
    public IValue Evaluate()
    {
        IValue conditionResult = Condition.Evaluate();
        return conditionResult.ToBool() ? TrueExpr.Evaluate() : FalseExpr.Evaluate();
    }
}