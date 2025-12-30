namespace DebtSystem.Models;

public class Creditor : Person
{
    public Creditor() { }

    public Creditor(string firstName, string lastName, string email = "")
        : base(firstName, lastName, email)
    {
    }
}