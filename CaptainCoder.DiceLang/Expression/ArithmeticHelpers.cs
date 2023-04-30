namespace CaptainCoder.DiceLang;
public static class ArithmeticHelpers
{
    public static IValue PerformOp(IValue leftVal, IValue rightVal, Func<INumericValue, INumericValue, INumericValue> op, string opName)
    {
        return (leftVal, rightVal) switch
        {
            (INumericValue left, INumericValue right) => op(left, right),
            _ => new ErrorValue($"Cannot perform {opName} on {leftVal.PrettyPrint()} and {rightVal.PrettyPrint()}"),
        };
    }
}