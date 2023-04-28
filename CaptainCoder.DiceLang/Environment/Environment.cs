namespace CaptainCoder.DiceLang;

public class Environment
{
    public static Environment Empty => new ();
    private readonly Dictionary<string, IValue> _environment;

    public Environment()
    {
        _environment = new();
    }

    private Environment(Dictionary<string, IValue> environment)
    {
        _environment = environment;
    }

    public IValue Lookup(string id) => _environment[id];

    public Environment Extend(string id, IValue expression)
    {
        Environment env = new (_environment.ToDictionary(entry => entry.Key, entry => entry.Value));
        env._environment[id] = expression;
        return env;
    }
}