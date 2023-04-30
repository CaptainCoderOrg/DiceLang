namespace CaptainCoder.DiceLang;
public record BoolValue(bool Value) : IExpression, IValue
{
    public IValue Evaluate(Environment env) => this;
    public string PrettyPrint() => Value.ToString();
    public ICastResult<int> ToInt() => CastError<int>.Error($"Cannot cast Bool to int.");
    public ICastResult<double> ToDouble() => CastError<double>.Error($"Cannot cast Bool to double.");
    public ICastResult<bool> ToBool() => new CastSuccess<bool>(Value);

}