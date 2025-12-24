namespace DebtSystem.Models;

/// <summary>
/// Класс кредитора — наследуется от Person.
/// </summary>
public class Creditor : Person
{
    public Creditor() { }

    public Creditor(string firstName, string lastName, string email = "")
        : base(firstName, lastName, email)
    {
    }
}