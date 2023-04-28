namespace CaptainCoder.DiceLang;
public record IntValue(int Value) : IExpression, IValue
{
    public IValue Evaluate(Environment env) => this;
    public int ToInt() => Value;
    public string PrettyPrint() => Value.ToString();
    public bool ToBool() => throw new NotSupportedException();
}