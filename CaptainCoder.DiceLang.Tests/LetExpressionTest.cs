using Sprache;
using Moq;
using CaptainCoder.Core;

namespace CaptainCoder.DiceLang.Tests;

public class LetExpressionTest
{
    [Fact]
    public void EvaluateSimpleSub()
    {
        // let x = 5 in x
        IExpression simpleLet = new LetExpression("x", new IntValue(5), new IdentifierValue("x"));

        IValue result = simpleLet.Evaluate();
        IValue expected = new IntValue(5);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void EvaluateSimpleSubWithArithmetic()
    {
        // let x = 5 in x + 2
        IExpression simpleLet = new LetExpression(
            "x", new IntValue(5), 
            new AdditionExpression(new IdentifierValue("x"), new IntValue(2)));

        IValue result = simpleLet.Evaluate();
        IValue expected = new IntValue(7);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void EvaluateNestedLetTest()
    {
        // let x = 5 in 
        // let y = 2 in x + y
        IExpression nestedLet = new LetExpression(
            "x", new IntValue(5), 
            new LetExpression("y", new IntValue(2),
            new AdditionExpression(new IdentifierValue("x"), new IdentifierValue("y"))));

        IValue result = nestedLet.Evaluate();
        IValue expected = new IntValue(7);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void EvaluateNestedLetWithNestingTest()
    {
        // let x = 5 in 
        // let y = 2 in x + y == y + x
        IExpression nestedLet = new LetExpression(
            "x", new IntValue(5), 
            new LetExpression("y", new IntValue(2),
            new EqualityExpression(
                new AdditionExpression(new IdentifierValue("x"), new IdentifierValue("y")),
                new AdditionExpression(new IdentifierValue("y"), new IdentifierValue("x")))));

        IValue result = nestedLet.Evaluate();
        IValue expected = new BoolValue(true);
        Assert.Equal(expected, result);
    }
}