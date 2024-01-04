using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnoEngine;

namespace WebApplication1.Pages.Play;

public class Index : PageModel
{
    private readonly DAL.AppDbContext _context;

    private readonly IGameRepository _gameRepository;
    private const string SpectatorGuidString = "00000000-0000-0000-0000-000000000000";
    public Guid SpectatorGuid { get; }= new Guid(SpectatorGuidString);
    public GameEngine Engine { get; set; } = default!;


    public Index(AppDbContext context)
    {
        _context = context;
        _gameRepository = new GameRepositoryEF(_context);
    }

    
    [BindProperty(SupportsGet = true)]
    public Guid GameId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid PlayerId { get; set; }

    public void OnGet()
    {
        if (GameId == Guid.Empty || (PlayerId == Guid.Empty && PlayerId != SpectatorGuid)) return;
        var gameState = _gameRepository.LoadGame(GameId);
        Engine = new GameEngine()
        {
            State = gameState
        };

    }
    
    public Player GetPlayer()
    {
        var player = Engine.State.Players.FirstOrDefault(p => PlayerId == p.Id);
        return player!;
    }
    
    public int GetPlayerNo()
    {
        var player = Engine.State.Players.FirstOrDefault(p => PlayerId == p.Id);
        return Engine.State.Players.IndexOf(player!);
    }
    
}