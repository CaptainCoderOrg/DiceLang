using Sprache;
using Moq;
using CaptainCoder.Core;

namespace CaptainCoder.DiceLang.Tests;

public class AdditionExpressionParserTest
{
    [Fact]
    public void Parse5Plus2()
    {
        // (5 + 2) + (8 + 6)

        // "5 + 2"
        IResult<IExpression> result = Parsers.ArithmeticExpression.TryParse("5 + 2");
        Assert.True(result.WasSuccessful);
        IValue evalResult = result.Value.Evaluate();
        IntValue expected = new (7);
        Assert.Equal(expected, evalResult);
    }

    [Fact]
    public void Parse5Plus2Plus9()
    {
        // (5 + 2) + (8 + 6)

        // "5 + 2"
        IResult<IExpression> result = Parsers.ArithmeticExpression.TryParse("5    +  2  +    9");
        Assert.True(result.WasSuccessful);
        IValue evalResult = result.Value.Evaluate();
        IntValue expected = new (16);
        Assert.Equal(expected, evalResult);
    }

    [Fact]
    public void Parse5Plus2d6()
    {
        // "5 + 2d6"
        Mock<IRandom> moqRandom = new ();
        DiceGroupExpression.DefaultRandomSource = moqRandom.Object;
        IResult<IExpression> result = Parsers.ArithmeticExpression.TryParse("5    +  2d6");
        Assert.True(result.WasSuccessful);
        moqRandom.SetupSequence((random) => random.Next(0, 6)).Returns(0).Returns(4); // 1 + 5
        IValue evalResult = result.Value.Evaluate();
        IntValue expected = new (11);
        Assert.Equal(expected, evalResult);
    }

        [Fact]
    public void Parse2d4Plus3d3()
    {
        // "5 + 2d6"
        Mock<IRandom> moqRandom = new ();
        DiceGroupExpression.DefaultRandomSource = moqRandom.Object;
        IResult<IExpression> result = Parsers.ArithmeticExpression.TryParse("2d4    +  3d3");
        Assert.True(result.WasSuccessful);
        moqRandom.SetupSequence((random) => random.Next(0, 4)).Returns(0).Returns(2); // 1 + 3
        moqRandom.SetupSequence((random) => random.Next(0, 3)).Returns(0).Returns(1).Returns(2); // 1 + 2 + 3
        IValue evalResult = result.Value.Evaluate();
        IntValue expected = new (10);
        Assert.Equal(expected, evalResult);
    }
}