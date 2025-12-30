using System.ComponentModel.DataAnnotations;

namespace DebtSystem.Models;

/// <summary>
/// Абстрактный базовый класс долга.
/// </summary>
public abstract class Debt
{
    public int Id { get; set; }

    public int DebtorId { get; set; }
    public Debtor? Debtor { get; set; }

    public int CreditorId { get; set; }
    public Creditor? Creditor { get; set; }

    public int CategoryId { get; set; }
    public DebtCategory? Category { get; set; }

    private decimal _amount;
    public decimal Amount
    {
        get => _amount;
        set => _amount = value < 0
            ? throw new ArgumentException("Сумма долга должна быть положительной.")
            : value;
    }

    public decimal RemainingAmount { get; set; }

    public DateTime IssueDate { get; set; } = DateTime.Now;
    public DateTime? DueDate { get; set; }

    public bool IsPaid => _amount == 0;

    public abstract decimal CalculatePenalty();

    protected Debt()
    {
        RemainingAmount = Amount;
    }

    protected Debt(decimal amount, DateTime? dueDate = null)
    {
        Amount = amount;
        RemainingAmount = amount;
        DueDate = dueDate;
    }
}

public class PersonalLoanDebt : Debt
{
    public PersonalLoanDebt() { }

    public PersonalLoanDebt(decimal amount, DateTime? dueDate = null)
        : base(amount, dueDate)
    {
    }

    public override decimal CalculatePenalty()
    {
        if (!DueDate.HasValue || DateTime.Now <= DueDate.Value)
            return 0;

        var daysLate = (DateTime.Now - DueDate.Value).TotalDays;
        if (daysLate <= 0) return 0;

        var monthsLate = (int)Math.Ceiling(daysLate / 30);
        return Amount * 0.05m * monthsLate;
    }
}
