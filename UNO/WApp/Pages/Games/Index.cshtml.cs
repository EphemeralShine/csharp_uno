using DAL;
using Domain.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnoEngine;

namespace WebApplication1.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly IGameRepository _repository;
        private readonly DAL.AppDbContext _context;
        private const string SpectatorGuidString = "00000000-0000-0000-0000-000000000000";
        public Guid SpectatorGuid { get; }= new Guid(SpectatorGuidString);

        public IndexModel(DAL.AppDbContext context, IGameRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public IList<Game> Game { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Game = await _context.Games
                .Include(g => g.Players)
                .OrderByDescending(g => g.UpdatedAtDt)
                .ToListAsync();
        }
        [BindProperty]
        public List<Player> Players { get; set; } = default!;
        [BindProperty]
        public bool MultipleCardMoves { get; set; }
        [BindProperty]
        public int CardAddition { get; set; }
        [BindProperty]
        public int HandSize { get; set; }

        public GameEngine Engine = default!;
        public async Task<IActionResult> OnPostAsync()
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
            foreach (var p in Players)
            {
                Engine.State.Players.Add(new Domain.Player()
                {
                    Name = p.NickName,
                    PlayerType = p.PlayerType
                });
            }
            Engine.InitializeGameStartingState();
            _repository.Save(Engine.State.Id, Engine.State);
            await _context.SaveChangesAsync();
            Game = await _context.Games
                .Include(g => g.Players)
                .OrderByDescending(g => g.UpdatedAtDt)
                .ToListAsync();
            return Page();
        }

    }
}
