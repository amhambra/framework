
namespace Signum.Entities.DynamicQuery;

public class Order: IEquatable<Order>
{
    public QueryToken Token { get;  }
    public OrderType OrderType { get; }

    public Order(QueryToken token, OrderType orderType)
    {
        this.Token = token;
        this.OrderType = orderType;
    }

    public override string ToString()
    {
        return "{0} {1}".FormatWith(Token.FullKey(), OrderType);
    }

    public override int GetHashCode() => Token.GetHashCode();
    public override bool Equals(object? obj) => obj is Order order && Equals(order);
    public bool Equals(Order? other) => other is Order o && o.Token.Equals(Token) && o.OrderType.Equals(OrderType);
}

[InTypeScript(true), DescriptionOptions(DescriptionOptions.Members | DescriptionOptions.Description)]
public enum OrderType
{
    Ascending,
    Descending
}
