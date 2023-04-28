namespace CaptainCoder.DiceLang;

public abstract record BinaryOperatorExpression(IExpression Left, IExpression Right) : IExpression
{

    public IValue Evaluate(Environment env)
    {
        IValue leftVal = Left.Evaluate(env);
        IValue rightVal = Right.Evaluate(env);
        return PerformOp(leftVal, rightVal);
    }
    protected abstract BinaryExpressionConstructor Constructor { get; }
    public abstract IValue PerformOp(IValue left, IValue right);
}

public delegate IExpression BinaryExpressionConstructor(IExpression left, IExpression right);