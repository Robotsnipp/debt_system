namespace DebtSystem.Models;

/// <summary>
/// Абстрактный базовый класс для человека (должника или кредитора).
/// </summary>
public abstract class Person
{
    public int Id { get; set; }

    private string _firstName;
    private string _lastName;
    private string _email;

    /// <summary>
    /// Имя человека. Не может быть пустым.
    /// </summary>
    public string FirstName
    {
        get => _firstName;
        set => _firstName = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Имя не может быть пустым.")
            : value.Trim();
    }

    /// <summary>
    /// Фамилия человека. Не может быть пустой.
    /// </summary>
    public string LastName
    {
        get => _lastName;
        set => _lastName = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Фамилия не может быть пустой.")
            : value.Trim();
    }

    /// <summary>
    /// Email (опционально). Должен содержать '@'.
    /// </summary>
    public string Email
    {
        get => _email;
        set => _email = string.IsNullOrWhiteSpace(value)
            ? ""
            : value.Trim().Contains("@")
                ? value.Trim()
                : throw new ArgumentException("Некорректный email.");
    }

    /// <summary>
    /// Полное имя (ФИО).
    /// </summary>
    public virtual string FullName => $"{FirstName} {LastName}";
    protected Person() { }

    /// <summary>
    /// Основной конструктор.
    /// </summary>
    public Person(string firstName, string lastName, string email = "")
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}