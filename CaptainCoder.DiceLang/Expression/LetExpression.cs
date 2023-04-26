using CaptainCoder.Core;
namespace CaptainCoder.DiceLang;


// New value type is identifier
// let LABEL = ValueExpr in BodyExpr
public record LetExpression(string Label, IExpression ValueExpr, IExpression BodyExpr) : IExpression
{
    public IValue Evaluate()
    {
        IValue value = ValueExpr.Evaluate();
        // return BodyExpr.Substitute(Label, value);
        throw new NotImplementedException();
    }

   public IExpression Substitute(string label, IExpression toSub)
   {
        if (label == Label)
        {
            throw new Exception($"Label {label} is already defined.");
        }
        return new LetExpression(Label, ValueExpr.Substitute(label, toSub), BodyExpr.Substitute(label, toSub));
   }
}

public record IdentifierValue(string Label) : IExpression, IValue
{
    public IValue Evaluate()
    {
        throw new Exception($"Encountered identifier '{Label}' without a let binding.");
    }

    public IExpression Substitute(string label, IExpression toSub)
    {
        if (label == Label)
        {
            return toSub;
        }
        return this;
    }

    public int ToInt() => throw new NotSupportedException();
    public bool ToBool() => throw new NotSupportedException();
    public string PrettyPrint() => Label;
}