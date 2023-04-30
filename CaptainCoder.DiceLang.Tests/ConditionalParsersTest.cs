using Sprache;
namespace CaptainCoder.DiceLang.Tests;

public class ConditionalParsersTest
{
    [Theory]
    [InlineData(3, 5)]
    [InlineData(2, 1)]
    [InlineData(7, 7)]
    [InlineData(5, 7)]
    public void TestGreaterThanExpr(int left, int right)
    {
        // 5 > 3
        // 2 > 1
        // 7 > 7
        IntValue leftVal = new (left);
        IntValue rightVal = new (right);
        GreaterThanExpression gtExpr = new (leftVal, rightVal);
        // bool expected = left > right;
        IExpression expected = new BoolValue(left > right);
        Assert.Equal(expected, gtExpr.Evaluate(Environment.Empty));
    }

    

        
    [Theory]
    [InlineData(3, 5)]
    [InlineData(2, 1)]
    [InlineData(7, 7)]
    [InlineData(5, 7)]
    public void TestGreaterThanParser(int left, int right)
    {
        // 5 > 3
        // 2 > 1
        // 7 > 7
        IntValue leftVal = new (left);
        IntValue rightVal = new (right);
        GreaterThanExpression gtExpr = new (leftVal, rightVal);

        IResult<IExpression> resultExpr = Parsers.DiceLangExpression.TryParse($"{left} > {right}");
        Assert.True(resultExpr.WasSuccessful);
        Assert.Equal(gtExpr, resultExpr.Value);
    }

    [Theory]
    [InlineData(3, 5)]
    [InlineData(2, 1)]
    [InlineData(7, 7)]
    [InlineData(5, 7)]
    public void TestGreaterThanOrEqualParser(int left, int right)
    {
        IntValue leftVal = new (left);
        IntValue rightVal = new (right);
        IResult<IExpression> resultExpr = Parsers.DiceLangExpression.TryParse($"{left} >= {right}");
        Assert.True(resultExpr.WasSuccessful);
        IExpression expected = new BoolValue(left >= right);
        Assert.Equal(expected, resultExpr.Value.Evaluate(Environment.Empty));
    }

    [Theory]
    [InlineData(3, 5)]
    [InlineData(2, 1)]
    [InlineData(7, 7)]
    [InlineData(5, 7)]
    public void TestLessThanOrEqualParser(int left, int right)
    {
        IntValue leftVal = new (left);
        IntValue rightVal = new (right);
        IResult<IExpression> resultExpr = Parsers.DiceLangExpression.TryParse($"{left} <= {right}");
        Assert.True(resultExpr.WasSuccessful);
        IExpression expected = new BoolValue(left <= right);
        Assert.Equal(expected, resultExpr.Value.Evaluate(Environment.Empty));
    }

    [Theory]
    [InlineData(3, 5)]
    [InlineData(2, 1)]
    [InlineData(7, 7)]
    [InlineData(5, 7)]
    public void TestNotEqualParser(int left, int right)
    {
        IntValue leftVal = new (left);
        IntValue rightVal = new (right);
        IResult<IExpression> resultExpr = Parsers.DiceLangExpression.TryParse($"{left} != {right}");
        Assert.True(resultExpr.WasSuccessful);
        IExpression expected = new BoolValue(left != right);
        Assert.Equal(expected, resultExpr.Value.Evaluate(Environment.Empty));
    }

    
}