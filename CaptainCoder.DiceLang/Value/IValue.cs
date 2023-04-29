namespace CaptainCoder.DiceLang;

public interface IValue : IExpression
{
    public ICastResult<int> ToInt();
    public bool ToBool();
    public string PrettyPrint();
}

// public delegate ICastResult<T> Caster<T>(IExpression expr);
public delegate ICastResult<OutT> NextAction<InT, OutT>(InT toPropogate);

public interface ICastResult<T> 
{ 
    public ICastResult<OutT> Then<OutT>(NextAction<T, OutT> action);
}
public record CastSuccess<T>(T Result) : ICastResult<T>
{
    public ICastResult<OutT> Then<OutT>(NextAction<T, OutT> action) => action(Result);
}

public record CastError<T>(string ErrorMessage) : ICastResult<T>
{
    public static CastError<T> Error(string message) => new (message);
    public ICastResult<OutT> Then<OutT>(NextAction<T, OutT> action) => CastError<OutT>.Error(ErrorMessage);
}