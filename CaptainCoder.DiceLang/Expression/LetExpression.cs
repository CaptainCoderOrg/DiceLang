using CaptainCoder.Core;
namespace CaptainCoder.DiceLang;


// New value type is identifier
// let LABEL = ValueExpr in BodyExpr
public record LetExpression(string Label, IExpression ValueExpr, IExpression BodyExpr) : IExpression
{
    public IValue Evaluate(Environment env)
    {
        IValue value = ValueExpr.Evaluate(env);
        if (value is FuncValue funcValue)
        {
            funcValue.Scope = funcValue.Scope.Extend(Label, value);
        }
        Environment extended = env.Extend(Label, value);
        return BodyExpr.Evaluate(extended);
    }

}

public record IdentifierValue(string Label) : IExpression, IValue
{
    public IValue Evaluate(Environment env) => env.Lookup(Label);
    public ICastResult<int> ToInt() => CastError<int>.Error($"Cannot cast Identifier {Label} to int.");
    public ICastResult<bool> ToBool() => CastError<bool>.Error($"Cannot cast Identifier {Label} to bool.");
    public string PrettyPrint() => Label;
}