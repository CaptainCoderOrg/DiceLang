namespace CaptainCoder.DiceLang;

public abstract record BinaryOperatorExpression(IExpression Left, IExpression Right) : IExpression
{
    
    public IValue Evaluate()
    {
        IValue leftVal = Left.Evaluate();
        IValue rightVal = Right.Evaluate();
        return PerformOp(leftVal, rightVal);
    }

    public abstract IValue PerformOp(IValue left, IValue right);
}