using Sprache;
using Moq;
using CaptainCoder.Core;

namespace CaptainCoder.DiceLang.Tests;

public class ListExpressionTest
{
    [Fact]
    public void ParseEmptyList()
    {
        // []
        IResult<IExpression> result = Parsers.ListExpr.TryParse("[]");
        Assert.True(result.WasSuccessful, $"Failed to parse with '{result.Message}'");

        IExpression expected = EmptyListExpr.Empty;
        Assert.Equal(expected, result.Value);

        IValue expectedVal = EmptyListValue.Empty;
        Assert.Equal(expectedVal, result.Value.Evaluate(Environment.Empty));
    }

    [Fact]
    public void ParseEmptyListWithWhitSpace()
    {
        // []
        IResult<IExpression> result = Parsers.ListExpr.TryParse("[   ]");
        Assert.True(result.WasSuccessful, $"Failed to parse with '{result.Message}'");

        IExpression expected = EmptyListExpr.Empty;
        Assert.Equal(expected, result.Value);

        IValue expectedVal = EmptyListValue.Empty;
        Assert.Equal(expectedVal, result.Value.Evaluate(Environment.Empty));
    }

    [Fact]
    public void ParseSingletonList()
    {
        // []
        IResult<IExpression> result = Parsers.ListExpr.TryParse("[1]");
        Assert.True(result.WasSuccessful, $"Failed to parse with '{result.Message}'");

        IExpression expected = new ListConsExpr(new IntValue(1), EmptyListExpr.Empty);
        Assert.Equal(expected, result.Value);

        IValue expectedVal = new ListCons(new IntValue(1), EmptyListValue.Empty);
        Assert.Equal(expectedVal, expected.Evaluate(Environment.Empty));
    }

    [Fact]
    public void ParseMultiList3()
    {
        // []
        IResult<IExpression> result = Parsers.ListExpr.TryParse("[   1,   'a',   true  ]");
        Assert.True(result.WasSuccessful, $"Failed to parse with '{result.Message}'");

        IExpression expected = 
        new ListConsExpr(new IntValue(1), 
        new ListConsExpr(new CharValue('a'),
        new ListConsExpr(new BoolValue(true),
            EmptyListExpr.Empty)));
        Assert.Equal(expected, result.Value);

        IValue expectedVal = 
            new ListCons(new IntValue(1), 
            new ListCons(new CharValue('a'), 
            new ListCons(new BoolValue(true), 
            EmptyListValue.Empty)));
        Assert.Equal(expectedVal, expected.Evaluate(Environment.Empty));
    }

    [Fact]
    public void ParseListWithExpr3()
    {
        // []
        IResult<IExpression> result = Parsers.ListExpr.TryParse("[   1 + 2,   5 < 7,   true && false  ]");
        Assert.True(result.WasSuccessful, $"Failed to parse with '{result.Message}'");

        IExpression expected = 
        new ListConsExpr( new AdditionExpression(new IntValue(1), new IntValue(2)),
        new ListConsExpr( new LessThanExpression(new IntValue(5), new IntValue(7)),
        new ListConsExpr( new AndExpression(new BoolValue(true), new BoolValue(false)),
            EmptyListExpr.Empty)));
        Assert.Equal(expected, result.Value);

        IValue expectedVal = 
            new ListCons(new IntValue(3), 
            new ListCons(new BoolValue(true), 
            new ListCons(new BoolValue(false), 
            EmptyListValue.Empty)));
        Assert.Equal(expectedVal, expected.Evaluate(Environment.Empty));
    }

    [Fact]
    public void PrettyPrint1_2ListTest()
    {
        // [ 1, 2 ]   
        IValue list = new ListCons(
            new IntValue(1),
            new ListCons(
                new IntValue(2),
                EmptyListValue.Empty
            )
        );
        Assert.Equal("[ 1, 2 ]", list.PrettyPrint());
    }

    [Fact]
    public void PrettyPrintEmptyList()
    {
        // [ 1, 2 ]   
        IValue list = EmptyListValue.Empty;
        Assert.Equal("[]", list.PrettyPrint());
    }

    [Fact]
    public void PrettyPrintSingletonList()
    {
        // [ 1 ]   
        IValue list = new ListCons(new IntValue(1), EmptyListValue.Empty);
        Assert.Equal("[ 1 ]", list.PrettyPrint());
    }

    [Fact]
    public void PrettyPrint3ValList()
    {
        // [ 1, 5, 'a' ]   
        IValue list = 
        new ListCons(new IntValue(1),
        new ListCons(new IntValue(5),
        new ListCons(new CharValue('a'), EmptyListValue.Empty)));
        Assert.Equal("[ 1, 5, 'a' ]", list.PrettyPrint());
    }
}