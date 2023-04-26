namespace CaptainCoder.DiceLang;
public record BoolValue(bool Value) : IExpression, IValue
{
    public IValue Evaluate() => this;
    public string PrettyPrint() => Value.ToString();
    public int ToInt() => throw new NotSupportedException();
    public bool ToBool() => Value;
}