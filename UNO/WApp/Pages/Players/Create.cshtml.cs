using System.ComponentModel.DataAnnotations;
using Domain.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnoEngine;

namespace WebApplication1.Pages.Players
{
    public class CreateModel : PageModel
    {
        public GameEngine Engine = new GameEngine();
        [BindProperty(SupportsGet = true)]
        public int MaxPlayerCount { get; set; }

        private readonly DAL.AppDbContext _context;
        [BindProperty (SupportsGet = true)] 
        public bool MultipleCardMoves { get; set; }
        
        [BindProperty (SupportsGet = true)]
        [Range(2, 19, ErrorMessage = "Value not in range.")]
        public int HandSize { get; set; }
        
        [BindProperty (SupportsGet = true)]
        [Range(1, 4, ErrorMessage = "Value not in range.")]
        public int CardAddition { get; set; }
        
        [BindProperty] 
        public int PlayerCount { get; set; }

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (TempData["RulesConfigured"] != null && (bool)(TempData["RulesConfigured"] ?? false))
            {
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = "Configure rules first!";
                return RedirectToPage("/Games/Create");
            }
        }
        
        public IActionResult OnPost()
        {
            if (CardAddition != 0 && HandSize != 0)
            {
                Engine = new GameEngine
                {
                    State =
                    {
                        Id = new Guid(),
                        GameRules =
                        {
                            MultipleCardMoves = MultipleCardMoves,
                            CardAddition = CardAddition,
                            HandSize = HandSize
                        }
                    }
                };
            }

            MaxPlayerCount = Engine.DetermineMaxPlayerCount();
            
            if (ModelState.IsValid && PlayerCount != 0)
            {
                TempData["RulesConfigured"] = true;
                TempData["PlayerCountConfigured"] = true;
                return RedirectToPage("/Players/Configure");
            }
            return Page();
        }
    }
}
