namespace CaptainCoder.DiceLang;
public record CharValue(char Value) : IExpression, INumericValue
{
    public IValue Evaluate(Environment env) => this;
    public string PrettyPrint() => $"'{Value}'";
    public ICastResult<int> ToInt() => new CastSuccess<int>(Value);
    public ICastResult<double> ToDouble() => CastError<double>.Error($"Cannot cast Char to double.");
    public ICastResult<bool> ToBool() => CastError<bool>.Error($"Cannot cast Char to Bool.");

    public INumericValue Add(INumericValue other)
    {
        return other switch
        {
            IntValue intVal => new CharValue((char)(Value + intVal.Value)),
            CharValue charValue => new CharValue((char)(Value + charValue.Value)),
            _ => new ErrorValue($"Cannot perform '{this.PrettyPrint()} + {other.PrettyPrint()}'"),
        };
    }

    public INumericValue Sub(INumericValue other)
    {
        return other switch
        {
            IntValue intVal => new CharValue((char)(Value - intVal.Value)),
            CharValue charValue => new CharValue((char)(Value - charValue.Value)),
            _ => new ErrorValue($"Cannot perform '{this.PrettyPrint()} - {other.PrettyPrint()}'"),
        };
    }

    public INumericValue Div(INumericValue other)
    {
        return other switch
        {
            IntValue intVal => new CharValue((char)(Value / intVal.Value)),
            CharValue charValue => new CharValue((char)(Value / charValue.Value)),
            _ => new ErrorValue($"Cannot perform '{this.PrettyPrint()} / {other.PrettyPrint()}'"),
        };
    }

    public INumericValue Mul(INumericValue other)
    {
        return other switch
        {
            IntValue intVal => new CharValue((char)(Value * intVal.Value)),
            CharValue charValue => new CharValue((char)(Value * charValue.Value)),
            _ => new ErrorValue($"Cannot perform '{this.PrettyPrint()} * {other.PrettyPrint()}'"),
        };
    }
}