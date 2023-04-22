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

public record AdditionExpression(Expression Left, Expression Right) : Expression
{
    public Value Evaluate()
    {
        Value leftVal = Left.Evaluate();
        Value rightVal = Right.Evaluate();
        return leftVal.Add(rightVal);
    }
}

