using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.Debts;

public class CreateModel : PageModel
{
    private readonly DebtSystemContext _context;

    public CreateModel(DebtSystemContext context)
    {
        _context = context;
    }

    public IList<Debtor> Debtors { get; set; } = new List<Debtor>();
    public IList<Creditor> Creditors { get; set; } = new List<Creditor>();
    public IList<DebtCategory> Categories { get; set; } = new List<DebtCategory>();

    [BindProperty]
    public PersonalLoanDebt Debt { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        Debtors = await _context.Debtors.ToListAsync();
        Creditors = await _context.Creditors.ToListAsync();
        Categories = await _context.DebtCategories.ToListAsync();

        if (!Debtors.Any() || !Creditors.Any() || !Categories.Any())
        {
            ModelState.AddModelError(string.Empty,
                "Нельзя создать долг: сначала добавьте хотя бы одного должника, кредитора и категорию.");
            return Page();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Debtors = await _context.Debtors.ToListAsync();
        Creditors = await _context.Creditors.ToListAsync();
        Categories = await _context.DebtCategories.ToListAsync();

        if (!ModelState.IsValid)
            return Page();

        try
        {
            if (!_context.Debtors.Any(d => d.Id == Debt.DebtorId) ||
                !_context.Creditors.Any(c => c.Id == Debt.CreditorId) ||
                !_context.DebtCategories.Any(cat => cat.Id == Debt.CategoryId))
            {
                ModelState.AddModelError(string.Empty, "Выбраны некорректные данные.");
                return Page();
            }

            Debt.RemainingAmount = Debt.Amount;

            _context.Debts.Add(Debt);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}