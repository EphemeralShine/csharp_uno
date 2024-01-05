using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UnoEngine;

namespace WebApplication1.Pages.Play;

public class Index : PageModel
{
    private readonly AppDbContext _context;

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
        while (Engine.IsPlayerAbleToMove() == false)
        {
            Engine.AddCardsToPlayer();
            _gameRepository.Save(Engine.State.Id, Engine.State);
        }
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

    public IActionResult OnPost(string moveListString)
    {
        List<GameCard> moveList = new List<GameCard>();
        var gameState = _gameRepository.LoadGame(GameId);
        Engine = new GameEngine()
        {
            State = gameState
        };
        if (moveListString == "AI")
        {
            if (Engine.IsGameOver() == false)
            {
                while (Engine.IsPlayerAbleToMove() == false)
                {
                    Engine.AddCardsToPlayer();
                    _gameRepository.Save(Engine.State.Id, Engine.State);
                }

                moveList = Engine.AIMove()!;
                Engine.UpdatePlayerHand(moveList);
                Engine.CardsAction(moveList);
                Engine.UpdateCardToBeat(moveList);
                Engine.State.Players[Engine.State.ActivePlayerNo].UnoImmune = false;
                Engine.UpdateActivePlayerNo();
                Engine.State.LastMove = moveList;
                _gameRepository.Save(Engine.State.Id, Engine.State);
                return RedirectToPage("Index", new { PlayerId, GameId });
            }
            else
            {
                return RedirectToPage("Index", new { PlayerId, GameId });
            }
        }
        if (moveListString == "UNO")
        {
            var res = Engine.Uno(PlayerId);
            _gameRepository.Save(Engine.State.Id, Engine.State);
            TempData["InfoMessage"] = res;
            return RedirectToPage("Index", new { PlayerId, GameId });
        }
        if (!moveListString.IsNullOrEmpty() && GetPlayerNo() == Engine.State.ActivePlayerNo)
        {
            moveList = JsonConvert.DeserializeObject<List<GameCard>>(moveListString)!;
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Something went wrong with the move, perhaps it was empty?");
        }
        if (Engine.IsGameOver() == false)
        {
            Engine.DetermineWinner();
            
            if (Engine.IsMoveValid(moveList) == false) 
            {
                ModelState.AddModelError(string.Empty, "Move is illegal, make sure first selected card beats current card to beat!");
            }
            else
            {
                Engine.UpdatePlayerHand(moveList);
                if (moveList[0].CardColor == ECardColor.Black)
                {
                    // TODO
                    ECardColor playerColorChange = ECardColor.None;
                    Engine.CardsAction(moveList, playerColorChange);
                }
                else
                {
                    Engine.CardsAction(moveList);    
                }
                Engine.UpdateCardToBeat(moveList);
                Engine.State.Players[Engine.State.ActivePlayerNo].UnoImmune = false;
                Engine.UpdateActivePlayerNo();
                Engine.State.LastMove = moveList;
                _gameRepository.Save(Engine.State.Id, Engine.State);
                TempData["InfoMessage"] = "Move successful!";
                while (Engine.IsPlayerAbleToMove() == false)
                {
                    Engine.AddCardsToPlayer();
                    _gameRepository.Save(Engine.State.Id, Engine.State);
                }
                /*if (Engine.State.Players[Engine.State.ActivePlayerNo].PlayerType == EPlayerType.Ai)
                {
                    moveList = [];
                    moveList = Engine.AIMove()!;
                    Engine.UpdatePlayerHand(moveList);
                    Engine.CardsAction(moveList);
                    Engine.UpdateCardToBeat(moveList);
                    Engine.UpdateActivePlayerNo();
                    _gameRepository.Save(Engine.State.Id, Engine.State);                    
                }*/
                return RedirectToPage("Index",new {PlayerId,GameId});
            }
        }
        else
        {
            return RedirectToPage("Index", new { PlayerId, GameId });
        }

        return Page();
    }
}