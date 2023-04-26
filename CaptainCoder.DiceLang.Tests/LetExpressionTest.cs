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

    [Fact]
    public void SimpleParseTest()
    {
        // let x = 5 in x
        IResult<IExpression> result = Parsers.LetExpr.TryParse("let x = 5 in x");
        Assert.True(result.WasSuccessful, $"Failed with '{result.Message}'");
        IExpression expected = new LetExpression("x", new IntValue(5), new IdentifierValue("x"));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void ParseIdentifierInEqExpr()
    {
        IResult<IExpression> result = Parsers.RelationalExpr.TryParse("x == y");
        Assert.True(result.WasSuccessful);
        IExpression expected = new EqualityExpression(new IdentifierValue("x"), new IdentifierValue("y"));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void ParseIdentifierArithmeticExpr()
    {
        IResult<IExpression> result = Parsers.ArithmeticExpression.TryParse("x + y");
        Assert.True(result.WasSuccessful);
        IExpression expected = new AdditionExpression(new IdentifierValue("x"), new IdentifierValue("y"));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void NestedLetParser()
    {
        IResult<IExpression> result = Parsers.LetExpr.TryParse(@"
        let x = 5 in
        let y = 2 in 
        x + y == y + x
        ");
        Assert.True(result.WasSuccessful);
        // let x = 5 in 
        // let y = 2 in x + y == y + x
        IExpression expected = new LetExpression(
            "x", new IntValue(5), 
            new LetExpression("y", new IntValue(2),
            new EqualityExpression(
                new AdditionExpression(new IdentifierValue("x"), new IdentifierValue("y")),
                new AdditionExpression(new IdentifierValue("y"), new IdentifierValue("x")))));

        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void ParseBoolValueExprFailWithPlus()
    {
        IResult<IExpression> result = Parsers.BoolValueExpression.TryParse("x + 2");
        Assert.False(result.WasSuccessful);
    }

    [Fact]
    public void ParseBoolFactorExprFailWithPlus()
    {
        IResult<IExpression> result = Parsers.BooleanFactorExpression.TryParse("x + 2");
        Assert.False(result.WasSuccessful);
    }

    [Fact]
    public void ParseIdentifierInArithmeticExprInLetExpr()
    {
        IResult<IExpression> result = Parsers.LetExpr.TryParse("let y = x + 2 in y");
        Assert.True(result.WasSuccessful, $"Parse failed with '{result.Message}'");
        IExpression expected = 
            new LetExpression(
                "y", new AdditionExpression(new IdentifierValue("x"), new IntValue(2)),
                new IdentifierValue("y")                
            );
        Assert.Equal(expected, result.Value);
    }
}