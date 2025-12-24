// Pages/People/Debtors/Index.cshtml.cs
using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.People.Debtors;

public class IndexModel : PageModel
{
    private readonly DebtSystemContext _context;

    public IndexModel(DebtSystemContext context)
    {
        _context = context;
    }

    public IList<Debtor> Debtors { get; set; } = new List<Debtor>();

    public async Task OnGetAsync()
    {
        Debtors = await _context.Debtors.ToListAsync();
    }
}