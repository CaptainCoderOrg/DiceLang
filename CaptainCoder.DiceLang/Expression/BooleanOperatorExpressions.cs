namespace CaptainCoder.DiceLang;

public record AndExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new AndExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) => new BoolValue(left.ToBool() && right.ToBool());
}

public record OrExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new OrExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) => new BoolValue(left.ToBool() || right.ToBool());
}

public record NotExpression(IExpression Expr) : IExpression
{
    public IValue Evaluate(Environment env) => new BoolValue(!Expr.Evaluate(env).ToBool());
}