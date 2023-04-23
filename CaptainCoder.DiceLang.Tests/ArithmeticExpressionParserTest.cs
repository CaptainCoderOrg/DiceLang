using Sprache;
using Moq;
using CaptainCoder.Core;

namespace CaptainCoder.DiceLang.Tests;

public class ArithmeticExpressionParserTest
{
    [Fact]
    public void ParseArithmeticExpression()
    {
        // "5 * 2 + 7 - 12 / 3"
        // 10 + 7 - 4
        // 17 - 4
        // 13
        IResult<IExpression> result = Parsers.ArithmeticExpression.TryParse("5 * 2 + 7 - 12 / 3");
        Assert.True(result.WasSuccessful);
        IValue evalResult = result.Value.Evaluate();
        IntValue expected = new (13);
        Assert.Equal(expected, evalResult);
    }

    [Fact]
    public void ParseAdditionSubtractionExpression()
    {
        IResult<IExpression> result = Parsers.ArithmeticExpression.TryParse("5 + 7 - 3");
        Assert.True(result.WasSuccessful);
        IValue evalResult = result.Value.Evaluate();
        IntValue expected = new (9);
        Assert.Equal(expected, evalResult);
    }

    [Fact]
    public void ParseMultiplicationExpression()
    {
        IResult<IExpression> result = Parsers.ArithmeticExpression.TryParse("5 * 3 * 2");
        Assert.True(result.WasSuccessful);
        IValue evalResult = result.Value.Evaluate();
        IntValue expected = new (30);
        Assert.Equal(expected, evalResult);
    }

    [Fact]
    public void ParseDivisionExpression()
    {
        IResult<IExpression> result = Parsers.ArithmeticExpression.TryParse("15 / 5");
        Assert.True(result.WasSuccessful);
        IValue evalResult = result.Value.Evaluate();
        IntValue expected = new (3);
        Assert.Equal(expected, evalResult);
    }

    [Fact]
    public void ParseTripleDivisionExpression()
    {
        // 2 * 3 * 5 * 7
        IResult<IExpression> result = Parsers.ArithmeticExpression.TryParse("210 / 7 / 5 / 2");
        Assert.True(result.WasSuccessful);
        IValue evalResult = result.Value.Evaluate();
        IntValue expected = new (3);
        Assert.Equal(expected, evalResult);
    }
}
/* 
    { DivisionExpression { 
        Left = IntValue { Value = 210 }, 
        Right = DivisionExpression { 
            Left = IntValue { Value = 7 }, 
            Right = DivisionExpression { 
                Left = IntValue { Value = 5 }, 
                Right = IntValue { Value = 2 } } } } }
*/