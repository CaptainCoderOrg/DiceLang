using CaptainCoder.DiceLang;
using Sprache;

List<string> Commands = new ();

// See https://aka.ms/new-console-template for more information
Console.Clear();
Console.WriteLine("DiceLang++ v0.0.0... Nothing works, be careful.");

// What does REPL stand for?
// Read Evaluate Print Loop
bool trueWuWu = true;
while (trueWuWu)
{
    DisplayPrompt();
    string input = Console.ReadLine()!;
    IResult<IExpression> result = Parsers.DiceLangExpression.TryParse(input);
    if (result.WasSuccessful)
    {
        IValue value = result.Value.Evaluate();
        Console.WriteLine(value.PrettyPrint());
    }
    else
    {
        Console.Error.WriteLine("Failed to parse:");
        Console.Error.WriteLine(result.Message);
    }
}

void DisplayPrompt()
{
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("DiceLang"); // DiceML
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.Write("++");
    Console.ResetColor();
    Console.Write(" > ");
}