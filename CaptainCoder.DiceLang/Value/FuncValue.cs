namespace CaptainCoder.DiceLang;
public record FuncValue(string ParameterId, IExpression BodyExpr) : IExpression, IValue
{
    public IValue Evaluate(Environment env) => this;
    // TODO: Might consider adding pretty print to IExpression
    public string PrettyPrint() => $"fun({ParameterId})";
    public int ToInt() => throw new NotSupportedException();
    public bool ToBool() => throw new NotSupportedException();

}

public record ApplyFuncExpression(IExpression FuncExpr, IExpression ArgExpr) : IExpression
{
    public IValue Evaluate(Environment env)
    {
        IValue value = FuncExpr.Evaluate(env);
        if (value is not FuncValue funcValue) { throw new NotSupportedException($"Cannot apply non-function expression: {value}"); }
        IValue argument = ArgExpr.Evaluate(env);
        Environment extended = env.Extend(funcValue.ParameterId, argument);
        // IExpression toExecute = funcValue.BodyExpr.Substitute(funcValue.ParameterId, argument);
        return funcValue.BodyExpr.Evaluate(extended);
    }

}