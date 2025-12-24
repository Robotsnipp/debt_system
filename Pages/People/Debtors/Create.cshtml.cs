using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DebtSystem.Pages.People.Debtors;

public class CreateModel : PageModel
{
    private readonly DebtSystemContext _context;

    public CreateModel(DebtSystemContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Debtor Debtor { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            _context.Debtors.Add(Debtor);
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