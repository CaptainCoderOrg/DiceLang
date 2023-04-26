namespace CaptainCoder.DiceLang;

public abstract record BinaryOperatorExpression(IExpression Left, IExpression Right) : IExpression
{

    public IValue Evaluate()
    {
        IValue leftVal = Left.Evaluate();
        IValue rightVal = Right.Evaluate();
        return PerformOp(leftVal, rightVal);
    }
    protected abstract BinaryExpressionConstructor Constructor { get; }
    public abstract IValue PerformOp(IValue left, IValue right);
    public IExpression Substitute(string label, IExpression toSub) => Constructor.Invoke(Left.Substitute(label, toSub), Right.Substitute(label, toSub));
}

public delegate IExpression BinaryExpressionConstructor(IExpression left, IExpression right);