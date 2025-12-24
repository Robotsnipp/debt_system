namespace DebtSystem.Models;

public class Debtor : Person
{
    public Debtor() { }

    public Debtor(string firstName, string lastName, string email = "")
        : base(firstName, lastName, email)
    {
    }
}