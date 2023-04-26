namespace CaptainCoder.DiceLang;

public record AndExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    public override IValue PerformOp(IValue left, IValue right) => new BoolValue(left.ToBool() && right.ToBool());
}

public record OrExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    public override IValue PerformOp(IValue left, IValue right) => new BoolValue(left.ToBool() || right.ToBool());
}

public record NotExpression(IExpression Expr) : IExpression
{
    public IValue Evaluate() => new BoolValue(!Expr.Evaluate().ToBool());
}