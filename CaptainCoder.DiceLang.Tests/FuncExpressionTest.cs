using Sprache;
using Moq;
using CaptainCoder.Core;

namespace CaptainCoder.DiceLang.Tests;

public class FuncExpressionTest
{
    [Fact]
    public void SimpleFuncEvaluation()
    {
        // fun(x) => x + 1
        IExpression func = new FuncValue("x", new AdditionExpression(new IdentifierValue("x"), new IntValue(1)));
        // f(7)
        IExpression apply = new ApplyFuncExpression(func, new IntValue(7));

        Assert.Equal(8, apply.Evaluate().ToInt());
    }

    [Fact]
    public void SimpleFuncParse()
    {
        IResult<IExpression> result = Parsers.FuncExpr.TryParse("fun(x) => x + 1");
        Assert.True(result.WasSuccessful);
        IExpression expected = new FuncValue("x", new AdditionExpression(new IdentifierValue("x"), new IntValue(1)));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void StaticScopingFunctionTest()
    {
        // let x = 5 in
        // let f = fun(x) => x + 1 in
        // f(1)
        IExpression expression = 
        new LetExpression("x", new IntValue(5),
            new LetExpression("f", new FuncValue("x", new AdditionExpression(new IdentifierValue("x"), new IntValue(1))),
                    new ApplyFuncExpression(new IdentifierValue("f"), new IntValue(1))));
        Assert.Equal(2, expression.Evaluate().ToInt());
    }

    [Fact]
    public void StaticScopingParseFuncTest()
    {
        // let x = 5 in
        // let f = fun(x) => x + 1 in
        // f(1)
        IResult<IExpression> result = Parsers.DiceLangExpression.TryParse(@"
        let x = 5 in
        let f = fun(x) => x + 1
        in f"
        );
        Assert.True(result.WasSuccessful, $"Parse failed with '{result.Message}'");
        IExpression expected = 
            new LetExpression("x", new IntValue(5),
                new LetExpression("f", new FuncValue("x", new AdditionExpression(new IdentifierValue("x"), new IntValue(1))),
                        new IdentifierValue("f")));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void SimpleApplicationParseTest()
    {
        IResult<IExpression> result = Parsers.ApplyFuncExpr.TryParse("f(1)");
        Assert.True(result.WasSuccessful, $"Parse failed with '{result.Message}");
        IExpression expected = new ApplyFuncExpression(new IdentifierValue("f"), new IntValue(1));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void AnonymousFunctionApplicationParseTest()
    {
        IResult<IExpression> result = Parsers.ApplyFuncExpr.TryParse("(fun (x) => x + 1)(1)");
        Assert.True(result.WasSuccessful, $"Parse failed with '{result.Message}");
        IExpression expected = new ApplyFuncExpression(
            new FuncValue("x", new AdditionExpression(new IdentifierValue("x"), new IntValue(1)))
            , new IntValue(1));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void DoubleApplicationParseTest()
    {
        IResult<IExpression> result = Parsers.ApplyFuncExpr.TryParse("f(1)(2)");
        Assert.True(result.WasSuccessful, $"Parse failed with '{result.Message}");
        IExpression expected = 
            new ApplyFuncExpression(
                new ApplyFuncExpression(
                    new IdentifierValue("f"), 
                    new IntValue(1)), 
                new IntValue(2));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void TestParseFuncInLet()
    {
        // let x = 5 in
        // let f = fun(x) => x + 1 in
        // f(1)
        IResult<IExpression> result = Parsers.DiceLangExpression.TryParse(@"
        let x = 3d6 + 7 in
        let y = 3d6 + x in
        let f = fun(x) => x + 1 in
        if x > y then x 
        else if x < y then 2
        else f (7)
        ");
        Assert.True(result.WasSuccessful, $"Parse failed with '{result.Message}'");
        // IExpression expected = 
        //     new LetExpression("x", new IntValue(5),
        //         new LetExpression("f", new FuncValue("x", new AdditionExpression(new IdentifierValue("x"), new IntValue(1))),
        //                 new IdentifierValue("f")));
        // Assert.Equal(expected, result.Value);
    }
    
}