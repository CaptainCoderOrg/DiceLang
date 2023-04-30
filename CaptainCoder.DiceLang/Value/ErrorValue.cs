namespace CaptainCoder.DiceLang;
public record ErrorValue(string Message) : IExpression, INumericValue
{
    public IValue Evaluate(Environment env) => this;
    public ICastResult<int> ToInt() => CastError<int>.Error($"Cannot cast Error to int.");
    public string PrettyPrint() => $"Error: \"{Message}\"";
    public ICastResult<bool> ToBool() => CastError<bool>.Error($"Cannot cast Error to Bool.");
    public ICastResult<double> ToDouble() => CastError<double>.Error($"Cannot cast Error to Double.");

    public INumericValue Add(INumericValue other) => this;
    public INumericValue Sub(INumericValue other) => this;
    public INumericValue Div(INumericValue other) => this;
    public INumericValue Mul(INumericValue other) => this;
}