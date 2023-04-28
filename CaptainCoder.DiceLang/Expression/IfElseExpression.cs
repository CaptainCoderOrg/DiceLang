using CaptainCoder.Core;
namespace CaptainCoder.DiceLang;


public record IfElseExpression(IExpression Condition, IExpression TrueExpr, IExpression FalseExpr) : IExpression
{
    public IValue Evaluate(Environment env)
    {
        IValue conditionResult = Condition.Evaluate(env);
        return conditionResult.ToBool() ? TrueExpr.Evaluate(env) : FalseExpr.Evaluate(env);
    }
}