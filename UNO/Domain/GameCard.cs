using Helpers;

namespace Domain;

public class GameCard
{
    public ECardColor CardColor{ get; set; }
    public ECardValue CardValue { get; set; }

    public override string ToString()
    {
        return CardColor.Description() + CardValue.Description();
    }

}