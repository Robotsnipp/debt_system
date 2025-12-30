using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.Debts;

public class IndexModel : PageModel
{
    private readonly DebtSystemContext _context;

    public IndexModel(DebtSystemContext context)
    {
        _context = context;
    }

    public IList<Debt> DebtList { get; set; } = new List<Debt>();

    public async Task OnGetAsync()
    {
        DebtList = await _context.Debts
            .Include(d => d.Debtor)
            .Include(d => d.Creditor)
            .Include(d => d.Category)
            .ToListAsync();
    }
}