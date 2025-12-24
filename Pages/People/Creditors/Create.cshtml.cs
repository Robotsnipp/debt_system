// Pages/People/Creditors/Create.cshtml.cs
using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DebtSystem.Pages.People.Creditors;

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
    public Creditor Creditor { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            _context.Creditors.Add(Creditor);
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