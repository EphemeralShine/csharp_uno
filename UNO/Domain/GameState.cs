namespace Domain;

public class GameState
{
    public int ActivePlayerNo { get; set; } = 0;
    public bool ClockwiseMoveOrder { get; set; } = true;
    public List<Player> Players { get; set; } = new();
    public GameCard? CardToBeat { get; set; }
    public ECardColor? CurrentColor { get; set; }
    public Queue<GameCard> CardsNotInPlay { get; set; } = new();
    
}
