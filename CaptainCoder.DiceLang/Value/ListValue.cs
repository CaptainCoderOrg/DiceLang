
using System.Text;

namespace CaptainCoder.DiceLang;

public interface IListExpr : IExpression {}
public record ListConsExpr(IExpression Head, IListExpr Tail) : IListExpr
{
    // TODO: The cast here feels incorrect
    public IValue Evaluate(Environment env) => new ListCons(Head.Evaluate(env), (IListValue)Tail.Evaluate(env));
}

public record EmptyListExpr : IListExpr
{
    public static readonly EmptyListExpr Empty = new ();
    public IValue Evaluate(Environment env) => EmptyListValue.Empty;
}


public abstract record IListValue : IValue
{
    public abstract IValue Evaluate(Environment env);
    public abstract string PrettyPrint();
    public ICastResult<bool> ToBool() => CastError<bool>.Error("Cannot cast list to bool");
    public ICastResult<double> ToDouble() => CastError<double>.Error("Cannot cast list to double");
    public ICastResult<int> ToInt() => CastError<int>.Error("Cannot cast list to int");
}
public record EmptyListValue : IListValue
{
    public static readonly EmptyListValue Empty = new ();
    public override IValue Evaluate(Environment env) => this;
    public override string PrettyPrint() => "[]";
}

public record ListCons(IValue Head, IListValue Tail) : IListValue
{
    public override IValue Evaluate(Environment env) => this;
    public override string PrettyPrint()
    {
        StringBuilder builder = new ();
        builder.Append($"[ {Head.PrettyPrint()}");
        IListValue tail = Tail;
        while (tail is ListCons tailCons)
        {
            builder.Append($", {tailCons.Head.PrettyPrint()}");
            tail = tailCons.Tail;
        }
        builder.Append(" ]");
        return builder.ToString();
    }    
}