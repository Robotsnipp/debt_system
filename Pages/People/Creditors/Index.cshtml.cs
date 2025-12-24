// Pages/People/Creditors/Index.cshtml.cs
using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.People.Creditors;

public class IndexModel : PageModel
{
    private readonly DebtSystemContext _context;

    public IndexModel(DebtSystemContext context)
    {
        _context = context;
    }

    public IList<Creditor> Creditors { get; set; } = new List<Creditor>();

    public async Task OnGetAsync()
    {
        Creditors = await _context.Creditors.ToListAsync();
    }
}