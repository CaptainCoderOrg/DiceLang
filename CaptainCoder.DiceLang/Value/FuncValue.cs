namespace CaptainCoder.DiceLang;
public record FuncValue(string ParameterId, IExpression BodyExpr) : IExpression, IValue
{
    public IValue Evaluate() => this;
    // TODO: Might consider adding pretty print to IExpression
    public string PrettyPrint() => $"fun({ParameterId})";
    public int ToInt() => throw new NotSupportedException();
    public bool ToBool() => throw new NotSupportedException();
    public IExpression Substitute(string label, IExpression toSub)
    {
        if (label == ParameterId) { return this; }
        return this with { BodyExpr = BodyExpr.Substitute(label, toSub) };
    }

}

public record ApplyFuncExpression(IExpression FuncExpr, IExpression ArgExpr) : IExpression
{
    public IValue Evaluate()
    {
        IValue value = FuncExpr.Evaluate();
        if (value is not FuncValue funcValue) { throw new NotSupportedException($"Cannot apply non-function expression: {value}"); }
        IValue argument = ArgExpr.Evaluate();
        IExpression toExecute = funcValue.BodyExpr.Substitute(funcValue.ParameterId, argument);
        return toExecute.Evaluate();
    }

    public IExpression Substitute(string label, IExpression toSub) => new ApplyFuncExpression(FuncExpr.Substitute(label, toSub), ArgExpr.Substitute(label, toSub));
}