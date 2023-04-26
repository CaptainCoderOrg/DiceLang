namespace CaptainCoder.DiceLang;

public interface IExpression
{
    public IValue Evaluate();
    public IExpression Substitute(string label, IExpression toSub);
}
