using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.Receipts;

public class DetailsModel : PageModel
{
    private readonly DebtSystemContext _context;

    public DetailsModel(DebtSystemContext context)
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
            .Include(d => d.Category)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (Debt == null) return NotFound();

        return Page();
    }
}