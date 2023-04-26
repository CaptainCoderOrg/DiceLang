namespace CaptainCoder.DiceLang;

public interface IValue
{
    public int ToInt();
    public bool ToBool();
    public string PrettyPrint();
}