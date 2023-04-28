using CaptainCoder.Core;
namespace CaptainCoder.DiceLang;
public record DiceGroupExpression(int DiceCount, int SideCount, IRandom RandomSource) : IExpression
{
    public static IRandom DefaultRandomSource { get; set; } = IRandom.Shared;
    public static DiceGroupExpression WithDefaultSource(int diceCount, int sideCount) => new (diceCount, sideCount, DefaultRandomSource);
    public IValue Evaluate(Environment env)
    {
        int sum = 0;
        for (int i = 0; i < DiceCount; i++)
        {
            sum += RandomSource.Next(0, SideCount) + 1;
        }
        return new IntValue(sum);
    }

}