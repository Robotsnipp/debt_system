using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.Payments;

public class CreateModel : PageModel
{
    private readonly DebtSystemContext _context;

    public CreateModel(DebtSystemContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Payment Payment { get; set; } = new();

    public Debt? Debt { get; set; }

    public async Task<IActionResult> OnGetAsync(int? debtId)
    {
        if (debtId == null) return NotFound();

        Debt = await _context.Debts.FindAsync(debtId);
        if (Debt == null || Debt.IsPaid) return NotFound();

        Payment.DebtId = debtId.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var debt = await _context.Debts.FindAsync(Payment.DebtId);
        if (debt == null)
        {
            ModelState.AddModelError(string.Empty, "Долг не найден.");
            return Page();
        }

        if (Payment.Amount <= 0)
        {
            ModelState.AddModelError(string.Empty, "Сумма выплаты должна быть больше нуля.");
            return Page();
        }

        if (Payment.Amount > debt.RemainingAmount)
        {
            ModelState.AddModelError(string.Empty,
                $"Сумма выплаты не может превышать остаток ({debt.RemainingAmount:C}).");
            return Page();
        }

        _context.Payments.Add(Payment);

        debt.RemainingAmount -= Payment.Amount;

        await _context.SaveChangesAsync();

        return RedirectToPage("/Debts/Index");
    }
}