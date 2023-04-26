using Sprache;
namespace CaptainCoder.DiceLang.Tests;

public class BooleanParsersTest
{
    [Fact]
    public void TestAndNotPrecedent()
    {
        IResult<IExpression> result = Parsers.BooleanExpression.TryParse("! true && ! false");
        Assert.True(result.WasSuccessful);
        IExpression expected = new AndExpression(new NotExpression(new BoolValue(true)), new NotExpression(new BoolValue(false)));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void TestAndOrPrecedent()
    {
        IResult<IExpression> result = Parsers.BooleanExpression.TryParse("true &&  false || false && true");
        Assert.True(result.WasSuccessful);
        IExpression expected = new OrExpression(
            new AndExpression(new BoolValue(true), new BoolValue(false)), 
            new AndExpression(new BoolValue(false), new BoolValue(true))
        );
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void TestComplexExpression()
    {
        IResult<IExpression> result = Parsers.BooleanExpression.TryParse("5 > 2 &&  6 < 1");
        Assert.True(result.WasSuccessful);
        IExpression expected = new AndExpression(
            new GreaterThanExpression(new IntValue(5), new IntValue(2)),
            new LessThanExpression(new IntValue(6), new IntValue(1))
        );
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void TestParenthesisNot()
    {
        IResult<IExpression> result = Parsers.BooleanExpression.TryParse("!(true && false)");
        Assert.True(result.WasSuccessful);
        IExpression expected = new NotExpression(new AndExpression(new BoolValue(true), new BoolValue(false)));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void TestMultiNot()
    {
        IResult<IExpression> result = Parsers.BooleanExpression.TryParse("!!!(true && false)");
        Assert.True(result.WasSuccessful);
        IExpression expected = new NotExpression(new NotExpression(new NotExpression(new AndExpression(new BoolValue(true), new BoolValue(false)))));
        Assert.Equal(expected, result.Value);
    }
}