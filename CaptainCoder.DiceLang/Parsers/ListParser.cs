using Sprache;
namespace CaptainCoder.DiceLang;

public static partial class Parsers
{
    // TODO: ArithmeticExpression is incorrect for true/false branch. Should be any expression.
    // let id = value in body
    public static Parser<IExpression> ListExpr =>
        from openBracket in Parse.Char('[').Token()
        from elements in ListElements.Optional()
        from closeBracket in Parse.Char(']').Token()
        select BuildList(elements.GetOrElse(new IExpression[0]));

    public static IListExpr BuildList(IEnumerable<IExpression> elements)
    {
        IListExpr list = EmptyListExpr.Empty;
        foreach (IExpression element in elements.Reverse())
        {
            list = new ListConsExpr(element, list);
        }
        return list;
    }

    public static Parser<IEnumerable<IExpression>> ListElements => 
        Parse.DelimitedBy(DiceLangExpressionInner.Token(), Parse.Char(',').Token());
    
}