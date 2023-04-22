using CaptainCoder.Core;
using Moq;
namespace CaptainCoder.DiceLang.Tests;

public class DiceGroupExpressionTest
{
    [Fact]
    public void TestSimpleDiceGroup()
    {
        Mock<IRandom> moqRandom = new ();
        moqRandom.Setup((random) => random.Next(0, 6)).Returns(2);
        // 3d6
        DiceGroupExpression _3d6 = new (3, 6, moqRandom.Object);
        Value result = _3d6.Evaluate();
        IntValue expected = new (9);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TestRoll2d4()
    {
        Mock<IRandom> moqRandom = new ();
        moqRandom.SetupSequence((random) => random.Next(0, 4))
            .Returns(2) // 3
            .Returns(0); // 1
        // 3d6
        DiceGroupExpression _2d4 = new (2, 4, moqRandom.Object);
        Value result = _2d4.Evaluate();
        IntValue expected = new (4);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TestRoll5d20()
    {
        Mock<IRandom> moqRandom = new ();
        moqRandom.SetupSequence((random) => random.Next(0, 20))
            .Returns(0) // 1
            .Returns(9) // 10
            .Returns(19) // 20
            .Returns(4) // 5
            .Returns(9); // 10
        // 3d6
        DiceGroupExpression _5d20 = new (5, 20, moqRandom.Object);
        Value result = _5d20.Evaluate();
        IntValue expected = new (46);
        Assert.Equal(expected, result);
    }
}