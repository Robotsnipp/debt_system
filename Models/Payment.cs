namespace DebtSystem.Models;

/// <summary>
/// Выплата по долгу.
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public int DebtId { get; set; }
    public Debt Debt { get; set; } = null!;

    private decimal _amount;
    public decimal Amount
    {
        get => _amount;
        set => _amount = value > 0
            ? value
            : throw new ArgumentException("Сумма выплаты должна быть положительной.");
    }

    public DateTime PaymentDate { get; set; } = DateTime.Now;
}