using System.ComponentModel.DataAnnotations;
using Domain.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)] public bool MultipleCardMoves { get; set; } = true;

        [BindProperty(SupportsGet = true)]
        [Range(2, 19, ErrorMessage = "Value not in range.")]
        public int HandSize { get; set; } = 7;

        [BindProperty(SupportsGet = true)]
        [Range(1, 4, ErrorMessage = "Value not in range.")]
        public int CardAddition { get; set; } = 2;

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                TempData["RulesConfigured"] = true;
                return RedirectToPage("/Players/Create");
            }  
            return Page();
        }
    }
}
