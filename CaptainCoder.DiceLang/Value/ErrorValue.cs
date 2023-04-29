namespace CaptainCoder.DiceLang;
public record ErrorValue(string Message) : IExpression, IValue
{
    public IValue Evaluate(Environment env) => this;
    public ICastResult<int> ToInt() => CastError<int>.Error($"Cannot cast Error to int.");
    public string PrettyPrint() => $"Error: \"{Message}\"";
    public bool ToBool() => throw new NotSupportedException();
}