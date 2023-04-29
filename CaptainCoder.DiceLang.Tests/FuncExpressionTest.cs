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
        IExpression expected = new IntValue(8);
        Assert.Equal(expected, apply.Evaluate(Environment.Empty));
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
        IExpression expected = new IntValue(2);
        Assert.Equal(expected, expression.Evaluate(Environment.Empty));
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
    }

    // [Fact]
    // public void OperatorParserTest()
    // {
    //     IResult<IEnumerable<char>> result = Parsers.Not.TryParse(" - ");
    // }

    [Fact]
    public void TestParseArithmeticNotAllowed()
    {
        IResult<IExpression> result = Parsers.ApplyFuncExpr.TryParse("x - 1");
        Assert.False(result.WasSuccessful);
        /*
                    ApplyFuncExpr
            .Or(ConditionalExpression)
            .Or(LetExpr)
            .Or(IfElseExpr)
            .Or(ArithmeticExpression)
            .Or(FuncExpr).Token();

            BoolIdentifier
            */
        result = Parsers.BoolIdentifier.TryParse("x - 1");
        Assert.False(result.WasSuccessful);

        result = Parsers.ConditionalExpression.TryParse("x - 1");
        Assert.False(result.WasSuccessful);
    }

    [Fact]
    public void TestParseXMinus1()
    {
        IResult<IExpression> result = Parsers.DiceLangExpressionInner.TryParse(@"
        x - 1
        ");
        Assert.True(result.WasSuccessful, $"Parse failed with '{result.Message}'");
        IExpression expected =
                new SubtractionExpression(new IdentifierValue("x"), new IntValue(1));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void TestParseApplyFuncWithXMinus1()
    {
        IResult<IExpression> result = Parsers.ApplyFuncExpr.TryParse(@"
        f (x - 1)
        ");
        Assert.True(result.WasSuccessful, $"Parse failed with '{result.Message}'");
        IExpression expected =
            new ApplyFuncExpression(
                new IdentifierValue("f"),
                new SubtractionExpression(new IdentifierValue("x"), new IntValue(1))
            );
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void TestParseFunInElse()
    {
        IResult<IExpression> result = Parsers.IfElseExpr.TryParse(@"
        if x == 0 then 1 else f (x - 1)
        ");
        Assert.True(result.WasSuccessful, $"Parse failed with '{result.Message}'");
        IExpression expected =
        new IfElseExpression(
            new EqualityExpression(new IdentifierValue("x"), new IntValue(0)),
            new IntValue(1),
            new ApplyFuncExpression(
                new IdentifierValue("f"),
                new SubtractionExpression(new IdentifierValue("x"), new IntValue(1))
            )
        );
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void TestParseNestedFunctionApplication()
    {
        IResult<IExpression> result = Parsers.DiceLangExpression.TryParse(@"
        let f = fun(x) => fun(y) => x + y
        in f (1)(2)
        ");
        Assert.True(result.WasSuccessful, $"Parse failed with '{result.Message}'");
        IExpression functions =
            new FuncValue("x",
                new FuncValue("y", new AdditionExpression(new IdentifierValue("x"), new IdentifierValue("y"))));
        IExpression expected =
        new LetExpression("f",
            functions,
            new ApplyFuncExpression(new ApplyFuncExpression(new IdentifierValue("f"), new IntValue(1)), new IntValue(2))
        );
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void TestExecuteNestedFunctionApplication()
    {
        IExpression functions =
            new FuncValue("x",
                new FuncValue("y", new AdditionExpression(new IdentifierValue("x"), new IdentifierValue("y"))));
        IExpression application =
            new ApplyFuncExpression(new ApplyFuncExpression(functions, new IntValue(1)), new IntValue(2));
        Assert.Equal(new IntValue(3), application.Evaluate(Environment.Empty));

    }

    [Fact]
    public void TestApplicationInBinaryOperator()
    {
        // exp(2) + 7
        string toParse = "exp(2) + 7";
        IResult<IExpression> result = Parsers.DiceLangExpression.TryParse(toParse);
        Assert.True(result.WasSuccessful, $"Failed to parse with '{result.Message}'");

        IExpression expected =
            new AdditionExpression(
                new ApplyFuncExpression(new IdentifierValue("exp"), new IntValue(2)),
                new IntValue(7)
            );
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void TestMultiParamFuncParse()
    {
        // exp(2) + 7
        string toParse = "fun(x, y) => 5";
        IResult<IExpression> result = Parsers.FuncExpr.TryParse(toParse);
        Assert.True(result.WasSuccessful, $"Failed to parse with '{result.Message}'");

        IExpression expected =
            new FuncValue("x",
                new FuncValue("y", new IntValue(5)));
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void Test5MultiParamFuncParse()
    {
        // exp(2) + 7
        string toParse = "fun(a, b, c, d, e) => 5";
        IResult<IExpression> result = Parsers.FuncExpr.TryParse(toParse);
        Assert.True(result.WasSuccessful, $"Failed to parse with '{result.Message}'");

        IExpression expected =
            new FuncValue("a",
            new FuncValue("b",
            new FuncValue("c",
            new FuncValue("d",
            new FuncValue("e", new IntValue(5))))));
        Assert.Equal(expected, result.Value);
    }
}