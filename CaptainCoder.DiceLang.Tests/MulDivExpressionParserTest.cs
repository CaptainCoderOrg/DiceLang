using Sprache;
using Moq;
using CaptainCoder.Core;

namespace CaptainCoder.DiceLang.Tests;

public class MulDivExpressionParserTest
{
    [Fact]
    public void ParseMul6Times3Times2()
    {
        IResult<IExpression> result = Parsers.MulDivExpression.TryParse("6 * 3 * 2");
        Assert.True(result.WasSuccessful);
        IValue evalResult = result.Value.Evaluate(Environment.Empty);
        IntValue expected = new (36);
        Assert.Equal(expected, evalResult);
    }

}