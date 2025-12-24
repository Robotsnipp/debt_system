// Pages/People/Debtors/Edit.cshtml.cs
using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.People.Debtors;

public class EditModel : PageModel
{
    private readonly DebtSystemContext _context;

    public EditModel(DebtSystemContext context)
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (Debtor == null) return NotFound();

        if (!ModelState.IsValid)
            return Page();

        try
        {
            _context.Attach(Debtor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DebtorExists(Debtor.Id))
                return NotFound();
            else
                throw;
        }

        return RedirectToPage("./Index");
    }

    private bool DebtorExists(int id)
    {
        return _context.Debtors.Any(e => e.Id == id);
    }
}