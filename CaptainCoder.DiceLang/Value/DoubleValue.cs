namespace CaptainCoder.DiceLang;
public record DoubleValue(double Value) : IExpression, IValue
{
    public IValue Evaluate(Environment env) => this;
    public ICastResult<int> ToInt() => new CastSuccess<int>((int)Value);
    public ICastResult<double> ToDouble() => new CastSuccess<double>(Value);
    public string PrettyPrint() => Value.ToString();
    public ICastResult<bool> ToBool() => CastError<bool>.Error($"Cannot cast Int to Bool.");
}