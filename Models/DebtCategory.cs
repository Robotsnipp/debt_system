namespace DebtSystem.Models;

/// <summary>
/// Категория долга (например: "Займ", "Коммуналка", "Учёба").
/// Используется для группировки и расчёта штрафов.
/// </summary>
public class DebtCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}