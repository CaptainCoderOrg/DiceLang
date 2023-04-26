namespace CaptainCoder.DiceLang;

public interface IValue : IExpression
{
    public int ToInt();
    public bool ToBool();
    public string PrettyPrint();
}