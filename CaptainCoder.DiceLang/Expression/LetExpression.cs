using CaptainCoder.Core;
namespace CaptainCoder.DiceLang;


// New value type is identifier
// let LABEL = ValueExpr in BodyExpr
public record LetExpression(string Label, IExpression ValueExpr, IExpression BodyExpr) : IExpression
{
    public IValue Evaluate(Environment env)
    {
        IValue value = ValueExpr.Evaluate(env);
        Environment extended = env.Extend(Label, value);
        return BodyExpr.Evaluate(extended);
    }

}

public record IdentifierValue(string Label) : IExpression, IValue
{
    public IValue Evaluate(Environment env) => env.Lookup(Label);

    public int ToInt() => throw new NotSupportedException();
    public bool ToBool() => throw new NotSupportedException();
    public string PrettyPrint() => Label;
}