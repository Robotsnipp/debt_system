using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.People.Creditors;

public class DeleteModel : PageModel
{
    private readonly DebtSystemContext _context;

    public DeleteModel(DebtSystemContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Creditor? Creditor { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        Creditor = await _context.Creditors.FindAsync(id);
        if (Creditor == null) return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();

        var creditor = await _context.Creditors.FindAsync(id);
        if (creditor == null) return NotFound();

        // Проверка: есть ли долги, связанные с этим кредитором?
        bool hasDebts = await _context.Debts.AnyAsync(d => d.CreditorId == id);
        if (hasDebts)
        {
            ModelState.AddModelError(string.Empty,
                "Нельзя удалить кредитора: существуют долги, связанные с ним.");
            Creditor = creditor;
            return Page();
        }

        _context.Creditors.Remove(creditor);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}