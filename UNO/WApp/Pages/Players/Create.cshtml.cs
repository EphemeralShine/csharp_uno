using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain.Database;
using Microsoft.VisualStudio.TextTemplating;
using UnoEngine;

namespace WebApplication1.Pages_Players
{
    public class CreateModel : PageModel
    {
        public GameEngine Engine = new GameEngine();

        private readonly DAL.AppDbContext _context;
        [BindProperty(SupportsGet = true)] 
        public bool MultipleCardMoves { get; set; } = true;
        
        [BindProperty(SupportsGet = true)]
        [Range(2, 19, ErrorMessage = "Value not in range.")]
        public int HandSize { get; set; } = 7;
        
        [BindProperty(SupportsGet = true)]
        [Range(1, 4, ErrorMessage = "Value not in range.")]
        public int CardAddition { get; set; } = 2;
        
        [BindProperty(SupportsGet = true)] 
        [Range(1, 10, ErrorMessage = "Value not in range.")]
        public int PlayerCount { get; set; } = 2;
        
        [BindProperty(SupportsGet = true)] 
        [Range(0, MaxPlayerCount, ErrorMessage = "Value not in range.")]
        public int AiCount { get; set; } = 0;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["GameId"] = new SelectList(_context.Games, "Id", "State");
            return Page();
        }

        [BindProperty]
        public Player Player { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Engine.State.GameRules.MultipleCardMoves = MultipleCardMoves;
            Engine.State.GameRules.CardAddition = CardAddition;
            Engine.State.GameRules.HandSize = HandSize;
            var MaxPlayerCount = Engine.DetermineMaxPlayerCount();
            
            
            _context.Players.Add(Player);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
