namespace CaptainCoder.DiceLang;

public record GreaterThanExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new GreaterThanExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) => 
        OperatorHelpers.PerformOp(left, right, (a, b) => a > b);
}

public record LessThanExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new LessThanExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) => 
        OperatorHelpers.PerformOp(left, right, (a, b) => a < b);
}

public record EqualityExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new EqualityExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) => 
        OperatorHelpers.PerformOp(left, right, (a, b) => a == b);
}