using CaptainCoder.Core;
namespace CaptainCoder.DiceLang;

public interface IValue
{
    public int ToInt();
    public bool ToBool();
    public string PrettyPrint();
}

public interface IExpression
{
    public IValue Evaluate();
}

public record IntValue(int Value) : IExpression, IValue
{
    public IValue Evaluate() => this;
    public int ToInt() => Value;
    public string PrettyPrint() => Value.ToString();
    public bool ToBool() => throw new NotSupportedException();
}

public record BoolValue(bool Value) : IExpression, IValue
{
    public IValue Evaluate() => this;
    public string PrettyPrint() => Value.ToString();
    public int ToInt() => throw new NotSupportedException();
    public bool ToBool() => Value;
}

public record DiceGroupExpression(int DiceCount, int SideCount, IRandom RandomSource) : IExpression
{
    public static IRandom DefaultRandomSource { get; set; } = IRandom.Shared;
    public static DiceGroupExpression WithDefaultSource(int diceCount, int sideCount) => new (diceCount, sideCount, DefaultRandomSource);
    public IValue Evaluate()
    {
        int sum = 0;
        for (int i = 0; i < DiceCount; i++)
        {
            sum += RandomSource.Next(0, SideCount) + 1;
        }
        return new IntValue(sum);
    }
}

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

public sealed record AdditionExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    public override IValue PerformOp(IValue left, IValue right) => new IntValue(left.ToInt() + right.ToInt());
}

public sealed record SubtractionExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    public override IValue PerformOp(IValue left, IValue right) => new IntValue(left.ToInt() - right.ToInt());
}

public sealed record MultiplicationExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    public override IValue PerformOp(IValue left, IValue right) => new IntValue(left.ToInt() * right.ToInt());
}

public sealed record DivisionExpression(IExpression Left, IExpression Right) : BinaryOperatorExpression(Left, Right)
{
    public override IValue PerformOp(IValue left, IValue right) => new IntValue(left.ToInt() / right.ToInt());
}