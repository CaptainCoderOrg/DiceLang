namespace CaptainCoder.DiceLang;
public record IdentifierValue(string Label) : IExpression, IValue
{
    public IValue Evaluate(Environment env) => env.Lookup(Label);
    public ICastResult<int> ToInt() => CastError<int>.Error($"Cannot cast Identifier {Label} to int.");
    public ICastResult<double> ToDouble() => CastError<double>.Error($"Cannot cast Identifier {Label} to double.");
    public ICastResult<bool> ToBool() => CastError<bool>.Error($"Cannot cast Identifier {Label} to bool.");
    public string PrettyPrint() => Label;
}