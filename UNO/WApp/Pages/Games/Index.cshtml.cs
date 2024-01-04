using Domain.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;
        private const string SpectatorGuidString = "00000000-0000-0000-0000-000000000000";
        public Guid SpectatorGuid { get; }= new Guid(SpectatorGuidString);

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Game> Game { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Game = await _context.Games
                .Include(g => g.Players)
                .OrderByDescending(g => g.UpdatedAtDt)
                .ToListAsync();
        }
    }
}
