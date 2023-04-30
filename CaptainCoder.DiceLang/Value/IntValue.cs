namespace CaptainCoder.DiceLang;
public record IntValue(int Value) : IExpression, INumericValue
{
    public IValue Evaluate(Environment env) => this;
    public ICastResult<int> ToInt() => new CastSuccess<int>(Value);
    public ICastResult<double> ToDouble() => new CastSuccess<double>(Value);
    public string PrettyPrint() => Value.ToString();
    public ICastResult<bool> ToBool() => CastError<bool>.Error($"Cannot cast Int to Bool.");

    public INumericValue Add(INumericValue other)
    {
        return other switch
        {
            IntValue intVal => new IntValue(Value + intVal.Value),
            DoubleValue doubleVal => new DoubleValue(Value + doubleVal.Value),
            CharValue charValue => new CharValue((char)(Value + (int)charValue.Value)),
            _ => new ErrorValue($"Cannot perform '{this.PrettyPrint()} + {other.PrettyPrint()}'"),
        };
    }

    public INumericValue Sub(INumericValue other)
    {
        return other switch
        {
            IntValue intVal => new IntValue(Value - intVal.Value),
            DoubleValue doubleVal => new DoubleValue(Value - doubleVal.Value),
            CharValue charValue => new CharValue((char)(Value - (int)charValue.Value)),
            _ => new ErrorValue($"Cannot perform '{this.PrettyPrint()} - {other.PrettyPrint()}'"),
        };
    }

    public INumericValue Div(INumericValue other)
    {
        return other switch
        {
            IntValue intVal => new IntValue(Value / intVal.Value),
            DoubleValue doubleVal => new DoubleValue(Value / doubleVal.Value),
            CharValue charValue => new CharValue((char)(Value / (int)charValue.Value)),
            _ => new ErrorValue($"Cannot perform '{this.PrettyPrint()} / {other.PrettyPrint()}'"),
        };
    }

    public INumericValue Mul(INumericValue other)
    {
        return other switch
        {
            IntValue intVal => new IntValue(Value * intVal.Value),
            DoubleValue doubleVal => new DoubleValue(Value * doubleVal.Value),
            CharValue charValue => new CharValue((char)(Value * (int)charValue.Value)),
            _ => new ErrorValue($"Cannot perform '{this.PrettyPrint()} * {other.PrettyPrint()}'"),
        };
    }
}