namespace CivCost.Domain.PurchaseOrders;

public sealed record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public Money(decimal amount, string currency = "EUR")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.");

        Amount = decimal.Round(amount, 2);
        Currency = currency;
    }

    public override string ToString() => $"{Amount:0.00} {Currency}";
}
