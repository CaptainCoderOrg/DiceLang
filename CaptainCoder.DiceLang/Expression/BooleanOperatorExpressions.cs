namespace CaptainCoder.DiceLang;

public record AndExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new AndExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) => 
        OperatorHelpers.PerformOp(left, right, (a, b) => a && b);
}

public record OrExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new OrExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) => 
        OperatorHelpers.PerformOp(left, right, (a, b) => a || b);
}

public record NotExpression(IExpression Expr) : IExpression
{
    public IValue Evaluate(Environment env)
    {
        ICastResult<bool> result = Expr.Evaluate(env).ToBool().Then((result) => new CastSuccess<bool>(!result));
        return result switch
        {
            CastError<bool>(string message) => new ErrorValue(message),
            CastSuccess<bool>(bool value) => new BoolValue(value),
            _ => throw new NotImplementedException(),
        };        
    }
}