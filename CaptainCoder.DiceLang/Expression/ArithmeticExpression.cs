namespace CaptainCoder.DiceLang;

public static class OperatorHelpers
{
    public static IValue PerformOp(IValue left, IValue right, Func<int, int, int> op)
    {
        ICastResult<int> result = left.ToInt()
            .Then((leftResult) => right.ToInt().Then((rightResult) => new CastSuccess<int>(op(leftResult, rightResult))));

        return result switch
        {
            CastError<int>(string message) => new ErrorValue(message),
            CastSuccess<int>(int value) => new IntValue(value),
            _ => throw new NotImplementedException(),
        };
    }

    public static IValue PerformOp(IValue left, IValue right, Func<int, int, bool> op)
    {
        ICastResult<bool> result = left.ToInt()
            .Then((leftResult) => right.ToInt().Then((rightResult) => new CastSuccess<bool>(op(leftResult, rightResult))));

        return result switch
        {
            CastError<bool>(string message) => new ErrorValue(message),
            CastSuccess<bool>(bool value) => new BoolValue(value),
            _ => throw new NotImplementedException(),
        };
    }

    public static IValue PerformOp(IValue left, IValue right, Func<bool, bool, bool> op)
    {
        ICastResult<bool> result = left.ToBool()
            .Then((leftResult) => right.ToBool().Then((rightResult) => new CastSuccess<bool>(op(leftResult, rightResult))));

        return result switch
        {
            CastError<bool>(string message) => new ErrorValue(message),
            CastSuccess<bool>(bool value) => new BoolValue(value),
            _ => throw new NotImplementedException(),
        };
    }
}

public sealed record AdditionExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new AdditionExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) =>
        OperatorHelpers.PerformOp(left, right, (a, b) => a + b);
}



public sealed record SubtractionExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new SubtractionExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) =>
        OperatorHelpers.PerformOp(left, right, (a, b) => a - b);
}

public sealed record MultiplicationExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new MultiplicationExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) =>
        OperatorHelpers.PerformOp(left, right, (a, b) => a * b);
}

public sealed record DivisionExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    protected override BinaryExpressionConstructor Constructor => (left, right) => new DivisionExpression(left, right);
    public override IValue PerformOp(IValue left, IValue right) =>
        OperatorHelpers.PerformOp(left, right, (a, b) => a / b);
}