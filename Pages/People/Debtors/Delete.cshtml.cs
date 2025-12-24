// Pages/People/Debtors/Delete.cshtml.cs
using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.People.Debtors;

public class DeleteModel : PageModel
{
    private readonly DebtSystemContext _context;

    public DeleteModel(DebtSystemContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Debtor? Debtor { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        Debtor = await _context.Debtors.FindAsync(id);
        if (Debtor == null) return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();

        var debtor = await _context.Debtors.FindAsync(id);
        if (debtor == null) return NotFound();

        // Проверка: есть ли долги, привязанные к этому должнику?
        bool hasDebts = await _context.Debts.AnyAsync(d => d.DebtorId == id);
        if (hasDebts)
        {
            ModelState.AddModelError(string.Empty,
                "Нельзя удалить должника: существуют долги, связанные с ним.");
            Debtor = debtor;
            return Page();
        }

        _context.Debtors.Remove(debtor);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}