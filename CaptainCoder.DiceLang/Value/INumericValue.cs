namespace CaptainCoder.DiceLang;
public interface INumericValue : IValue
{
    public INumericValue Add(INumericValue other);
    public INumericValue Sub(INumericValue other);
    public INumericValue Div(INumericValue other);
    public INumericValue Mul(INumericValue other);

    public static INumericValue operator +(INumericValue a, INumericValue b) => a.Add(b);
    public static INumericValue operator -(INumericValue a, INumericValue b) => a.Sub(b);
    public static INumericValue operator /(INumericValue a, INumericValue b) => a.Div(b);
    public static INumericValue operator *(INumericValue a, INumericValue b) => a.Mul(b);
}