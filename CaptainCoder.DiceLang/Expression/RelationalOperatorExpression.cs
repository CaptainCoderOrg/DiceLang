namespace CaptainCoder.DiceLang;

public record GreaterThanExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    public override IValue PerformOp(IValue left, IValue right) => new BoolValue(left.ToInt() > right.ToInt());
}

public record LessThanExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    public override IValue PerformOp(IValue left, IValue right) => new BoolValue(left.ToInt() < right.ToInt());
}

public record EqualityExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    public override IValue PerformOp(IValue left, IValue right) => new BoolValue(left.ToInt() == right.ToInt());
}