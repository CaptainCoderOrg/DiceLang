namespace CaptainCoder.DiceLang;
public static class ArithmeticHelpers
{
    public static IValue PerformAddition(IValue leftVal, IValue rightVal)
    {
        return (leftVal, rightVal) switch
        {
            (IntValue left, IntValue right) => PerformAddition(left, right),
            (IntValue left, DoubleValue right) => PerformAddition(left, right),
            (DoubleValue left, IntValue right) => PerformAddition(left, right),
            (DoubleValue left, DoubleValue right) => PerformAddition(left, right),
            _ => new ErrorValue($"Cannot PerformAddition addition on {leftVal.PrettyPrint()} and {rightVal.PrettyPrint()}"),
        };
    }
    
    public static IValue PerformAddition(IntValue left, IntValue right) => new IntValue(left.Value + right.Value);
    public static IValue PerformAddition(IntValue left, DoubleValue right) => new DoubleValue(left.Value + right.Value);
    public static IValue PerformAddition(DoubleValue left, DoubleValue right) => new DoubleValue(left.Value + right.Value);
    public static IValue PerformAddition(DoubleValue left, IntValue right) => new DoubleValue(left.Value + right.Value);

    public static IValue PerformSubtraction(IValue leftVal, IValue rightVal)
    {
        return (leftVal, rightVal) switch
        {
            (IntValue left, IntValue right) => PerformSubtraction(left, right),
            (IntValue left, DoubleValue right) => PerformSubtraction(left, right),
            (DoubleValue left, IntValue right) => PerformSubtraction(left, right),
            (DoubleValue left, DoubleValue right) => PerformSubtraction(left, right),
            _ => new ErrorValue($"Cannot PerformSubtraction addition on {leftVal.PrettyPrint()} and {rightVal.PrettyPrint()}"),
        };
    }
    
    public static IValue PerformSubtraction(IntValue left, IntValue right) => new IntValue(left.Value - right.Value);
    public static IValue PerformSubtraction(IntValue left, DoubleValue right) => new DoubleValue(left.Value - right.Value);
    public static IValue PerformSubtraction(DoubleValue left, DoubleValue right) => new DoubleValue(left.Value - right.Value);
    public static IValue PerformSubtraction(DoubleValue left, IntValue right) => new DoubleValue(left.Value - right.Value);

    public static IValue PerformMultiplication(IValue leftVal, IValue rightVal)
    {
        return (leftVal, rightVal) switch
        {
            (IntValue left, IntValue right) => PerformMultiplication(left, right),
            (IntValue left, DoubleValue right) => PerformMultiplication(left, right),
            (DoubleValue left, IntValue right) => PerformMultiplication(left, right),
            (DoubleValue left, DoubleValue right) => PerformMultiplication(left, right),
            _ => new ErrorValue($"Cannot PerformMultiplication addition on {leftVal.PrettyPrint()} and {rightVal.PrettyPrint()}"),
        };
    }
    
    public static IValue PerformMultiplication(IntValue left, IntValue right) => new IntValue(left.Value * right.Value);
    public static IValue PerformMultiplication(IntValue left, DoubleValue right) => new DoubleValue(left.Value * right.Value);
    public static IValue PerformMultiplication(DoubleValue left, DoubleValue right) => new DoubleValue(left.Value * right.Value);
    public static IValue PerformMultiplication(DoubleValue left, IntValue right) => new DoubleValue(left.Value * right.Value);

    public static IValue PerformDivision(IValue leftVal, IValue rightVal)
    {
        return (leftVal, rightVal) switch
        {
            (IntValue left, IntValue right) => PerformDivision(left, right),
            (IntValue left, DoubleValue right) => PerformDivision(left, right),
            (DoubleValue left, IntValue right) => PerformDivision(left, right),
            (DoubleValue left, DoubleValue right) => PerformDivision(left, right),
            _ => new ErrorValue($"Cannot PerformDivision addition on {leftVal.PrettyPrint()} and {rightVal.PrettyPrint()}"),
        };
    }
    
    public static IValue PerformDivision(IntValue left, IntValue right) => new IntValue(left.Value / right.Value);
    public static IValue PerformDivision(IntValue left, DoubleValue right) => new DoubleValue(left.Value / right.Value);
    public static IValue PerformDivision(DoubleValue left, DoubleValue right) => new DoubleValue(left.Value / right.Value);
    public static IValue PerformDivision(DoubleValue left, IntValue right) => new DoubleValue(left.Value / right.Value);
}