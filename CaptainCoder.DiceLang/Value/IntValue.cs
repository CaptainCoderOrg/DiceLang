namespace CaptainCoder.DiceLang;
public record IntValue(int Value) : IExpression, IValue
{
    public IValue Evaluate(Environment env) => this;
    public ICastResult<int> ToInt() => new CastSuccess<int>(Value);
    public string PrettyPrint() => Value.ToString();
    public ICastResult<bool> ToBool() => CastError<bool>.Error($"Cannot cast Int to Bool.");
}