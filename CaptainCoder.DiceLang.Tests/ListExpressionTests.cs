using Sprache;
using Moq;
using CaptainCoder.Core;

namespace CaptainCoder.DiceLang.Tests;

public class ListExpressionTest
{
    [Fact]
    public void PrettyPrint1_2ListTest()
    {
        // [ 1, 2 ]   
        IValue list = new ListCons(
            new IntValue(1),
            new ListCons(
                new IntValue(2),
                EmptyList.Empty
            )
        );
        Assert.Equal("[ 1, 2 ]", list.PrettyPrint());
    }

    [Fact]
    public void PrettyPrintEmptyList()
    {
        // [ 1, 2 ]   
        IValue list = EmptyList.Empty;
        Assert.Equal("[]", list.PrettyPrint());
    }

    [Fact]
    public void PrettyPrintSingletonList()
    {
        // [ 1 ]   
        IValue list = new ListCons(new IntValue(1), EmptyList.Empty);
        Assert.Equal("[ 1 ]", list.PrettyPrint());
    }

    [Fact]
    public void PrettyPrint3ValList()
    {
        // [ 1, 5, 'a' ]   
        IValue list = 
        new ListCons(new IntValue(1),
        new ListCons(new IntValue(5),
        new ListCons(new CharValue('a'), EmptyList.Empty)));
        Assert.Equal("[ 1, 5, 'a' ]", list.PrettyPrint());
    }
}