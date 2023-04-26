namespace CaptainCoder.DiceLang;

public sealed record AdditionExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new AdditionExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) => new IntValue(left.ToInt() + right.ToInt());

}

public sealed record SubtractionExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new SubtractionExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) => new IntValue(left.ToInt() - right.ToInt());
}

public sealed record MultiplicationExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new MultiplicationExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) => new IntValue(left.ToInt() * right.ToInt());
}

public sealed record DivisionExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new DivisionExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) => new IntValue(left.ToInt() / right.ToInt());
}