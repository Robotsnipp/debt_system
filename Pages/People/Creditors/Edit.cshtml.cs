using DebtSystem.Data;
using DebtSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Pages.People.Creditors;

public class EditModel : PageModel
{
    private readonly DebtSystemContext _context;

    public EditModel(DebtSystemContext context)
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (Creditor == null) return NotFound();

        if (!ModelState.IsValid)
            return Page();

        try
        {
            _context.Attach(Creditor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CreditorExists(Creditor.Id))
                return NotFound();
            else
                throw;
        }

        return RedirectToPage("./Index");
    }

    private bool CreditorExists(int id)
    {
        return _context.Creditors.Any(e => e.Id == id);
    }
}