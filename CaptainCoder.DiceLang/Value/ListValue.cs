
using System.Text;

namespace CaptainCoder.DiceLang;

public abstract record IListValue : IValue
{
    public abstract IValue Evaluate(Environment env);
    public abstract string PrettyPrint();
    public ICastResult<bool> ToBool() => CastError<bool>.Error("Cannot cast list to bool");
    public ICastResult<double> ToDouble() => CastError<double>.Error("Cannot cast list to double");
    public ICastResult<int> ToInt() => CastError<int>.Error("Cannot cast list to int");
}
public record EmptyList : IListValue
{
    public static readonly EmptyList Empty = new ();
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