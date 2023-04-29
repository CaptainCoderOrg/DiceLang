using CaptainCoder.Core;
namespace CaptainCoder.DiceLang;


public record IfElseExpression(IExpression Condition, IExpression TrueExpr, IExpression FalseExpr) : IExpression
{
    public IValue Evaluate(Environment env)
    {
        IValue conditionResult = Condition.Evaluate(env);
        ICastResult<bool> result = conditionResult.ToBool();
        return result switch
        {
            CastError<bool>(string message) => new ErrorValue(message),
            CastSuccess<bool>(bool value) => value ? TrueExpr.Evaluate(env) : FalseExpr.Evaluate(env),
            _ => throw new NotImplementedException(),
        };
    }
}