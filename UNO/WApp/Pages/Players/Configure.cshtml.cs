using Domain.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnoEngine;

namespace WebApplication1.Pages.Players;

public class Configure : PageModel
{
    public List<Player> Players { get; set; } = default!;
    [BindProperty]
    public int PlayerCount { get; set; }
    [BindProperty]
    public bool MultipleCardMoves { get; set; }
    [BindProperty]
    public bool CardAddition { get; set; }
    [BindProperty]
    public bool HandSize { get; set; }
    [BindProperty]
    public int MaxPlayerCount { get; set; }

    public object EPlayerType { get; set; } = default!;

    public IActionResult OnGet()
    {
        if (TempData["PlayerCountConfigured"] != null && (bool)(TempData["PlayerCountConfigured"] ?? false))
        {
            return Page();
        }
        else
        {
            TempData["ErrorMessage"] = "Configure player count and rules first!";
            return RedirectToPage("/Games/Create");
        }
    }

    public IActionResult OnPost()
    {
        // Validate PlayerCount dynamically
        if (PlayerCount < 2 || PlayerCount > MaxPlayerCount)
        {
            TempData["ErrorMessage"] = $"PlayerCount must be between 2 and {MaxPlayerCount}.";
            TempData["RulesConfigured"] = true;
            return RedirectToPage("Create",new { MaxPlayerCount , MultipleCardMoves, CardAddition, HandSize});;
        }
        /*_context.Players.Add(Player);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");*/
        return Page();
    }
}
