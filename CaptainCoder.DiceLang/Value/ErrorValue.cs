namespace CaptainCoder.DiceLang;
public record ErrorValue(string Message) : IExpression, IValue
{
    public IValue Evaluate(Environment env) => this;
    public ICastResult<int> ToInt() => CastError<int>.Error($"Cannot cast Error to int.");
    public string PrettyPrint() => $"Error: \"{Message}\"";
    public ICastResult<bool> ToBool() => CastError<bool>.Error($"Cannot cast Error to Bool.");
    public ICastResult<double> ToDouble() => CastError<double>.Error($"Cannot cast Error to Double.");
}