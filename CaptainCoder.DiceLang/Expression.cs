using CaptainCoder.Core;
using Sprache;
namespace CaptainCoder.DiceLang;
public class Class1
{

}

public interface Value
{
    public Value Add(Value other);
}

public interface Expression
{
    public Value Evaluate();
}

public record IntValue(int Value) : Expression, Value
{
    public Value Evaluate() => this;

    public Value Add(Value other)
    {
        if (other is IntValue otherInt)
        {
            return new IntValue(Value + otherInt.Value);
        }
        throw new ArgumentException($"Cannot perform addition between {nameof(IntValue)} and {other.GetType()}");
    }
}

public record DiceGroupExpression(int DiceCount, int SideCount, IRandom randomSource) : Expression
{
    public static IRandom DefaultRandomSource { get; set; } = IRandom.Shared;
    public static DiceGroupExpression WithDefaultSource(int diceCount, int sideCount) => new (diceCount, sideCount, DefaultRandomSource);
    public Value Evaluate()
    {
        int sum = 0;
        for (int i = 0; i < DiceCount; i++)
        {
            sum += randomSource.Next(0, SideCount) + 1;
        }
        return new IntValue(sum);
    }
}

public record AdditionExpression(Expression Left, Expression Right) : Expression
{
    public Value Evaluate()
    {
        Value leftVal = Left.Evaluate();
        Value rightVal = Right.Evaluate();
        return leftVal.Add(rightVal);
    }
}

