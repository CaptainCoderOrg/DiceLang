namespace CaptainCoder.DiceLang.Tests;

public class AdditionExpressionTest
{
    [Fact]
    public void TestSimpleAddition()
    {
        // (5 + 2) + (8 + 6)

        // (5 + 2)
        AdditionExpression leftExpr = new (new IntValue(5), new IntValue(2));

        // (8 + 6)
        AdditionExpression rightExpr = new (new IntValue(8), new IntValue(6));
        
        AdditionExpression additionExpr = new (leftExpr, rightExpr);

        Value result = additionExpr.Evaluate();
        Value expected = new IntValue(21);
        Assert.Equal(expected, result);
    }
}