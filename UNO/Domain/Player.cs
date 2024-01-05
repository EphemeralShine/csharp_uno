namespace Domain;

public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public EPlayerType PlayerType { get; set; }
    
    public List<GameCard> PlayerHand { get; set; } = new();

    public bool UnoImmune { get; set; } = false;

}