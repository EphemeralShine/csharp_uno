namespace Domain;

public class GameState
{
    public int ActivePlayerNo { get; set; } = 0; // TODO: how to choose the starter?
    public List<Player> Players { get; set; } = new();
    public GameCard? CardToBeat { get; set; }
    public Queue<GameCard> CardsNotInPlay { get; set; } = new();
}
