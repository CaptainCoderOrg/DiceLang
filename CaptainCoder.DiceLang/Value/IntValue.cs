namespace CaptainCoder.DiceLang;
public record IntValue(int Value) : IExpression, IValue
{
    public IValue Evaluate() => this;
    public int ToInt() => Value;
    public string PrettyPrint() => Value.ToString();
    public bool ToBool() => throw new NotSupportedException();
    public IExpression Substitute(string label, IExpression toSub) => this;
}