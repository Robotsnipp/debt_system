// Pages/Debts/Edit.cshtml.cs
using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.Debts;

public class EditModel : PageModel
{
    private readonly DebtSystemContext _context;

    public EditModel(DebtSystemContext context)
    {
        _context = context;
    }

    [BindProperty]
    public PersonalLoanDebt? Debt { get; set; }

    // —писки дл€ выпадающих меню
    public IList<Debtor> Debtors { get; set; } = new List<Debtor>();
    public IList<Creditor> Creditors { get; set; } = new List<Creditor>();
    public IList<DebtCategory> Categories { get; set; } = new List<DebtCategory>();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        // «агружаем долг с типом PersonalLoanDebt (т.к. мы используем наследование)
        Debt = await _context.Debts
            .OfType<PersonalLoanDebt>()
            .Include(d => d.Debtor)
            .Include(d => d.Creditor)
            .Include(d => d.Category)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (Debt == null) return NotFound();

        // «агружаем справочники
        Debtors = await _context.Debtors.ToListAsync();
        Creditors = await _context.Creditors.ToListAsync();
        Categories = await _context.DebtCategories.ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Debt == null) return NotFound();

        // ѕерезагружаем справочники дл€ валидации
        Debtors = await _context.Debtors.ToListAsync();
        Creditors = await _context.Creditors.ToListAsync();
        Categories = await _context.DebtCategories.ToListAsync();

        if (!ModelState.IsValid)
            return Page();

        try
        {
            // ¬алидаци€ существовани€ св€занных сущностей
            if (!_context.Debtors.Any(d => d.Id == Debt.DebtorId) ||
                !_context.Creditors.Any(c => c.Id == Debt.CreditorId) ||
                !_context.DebtCategories.Any(cat => cat.Id == Debt.CategoryId))
            {
                ModelState.AddModelError(string.Empty, "¬ыбраны некорректные данные.");
                return Page();
            }

            // ќбновл€ем только пол€, которые можно редактировать
            // (оставл€ем IssueDate без изменений Ч он фиксирован при создании)
            _context.Entry(Debt).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DebtExists(Debt.Id))
                return NotFound();
            else
                throw;
        }
    }

    private bool DebtExists(int id)
    {
        return _context.Debts.Any(e => e.Id == id);
    }
}