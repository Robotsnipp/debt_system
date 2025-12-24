using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.Debts;

public class DeleteModel : PageModel
{
    private readonly DebtSystemContext _context;

    public DeleteModel(DebtSystemContext context)
    {
        _context = context;
    }

    public Debt? Debt { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        Debt = await _context.Debts
            .Include(d => d.Debtor)
            .Include(d => d.Creditor)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (Debt == null) return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();

        var debt = await _context.Debts.FindAsync(id);
        if (debt != null)
        {
            _context.Debts.Remove(debt);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}