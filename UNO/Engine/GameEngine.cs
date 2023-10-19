using System.Runtime.InteropServices;
using Domain;

namespace UnoEngine;

public class GameEngine
{
    private Random Rnd { get; set; } = new Random();
    public GameState State { get; set; } = new GameState();

    public GameEngine()
    {
        PrepareDecks();
    }
    
    private void PrepareDecks()
    {
        var playingCards = new List<GameCard>();
        for (int cardValue = 1; cardValue < (int)ECardValue.Add4; cardValue++)
        {
            for (int cardColor = 0; cardColor < (int)ECardColor.Black; cardColor++)
            {
                playingCards.Add(new GameCard()
                {
                    CardValue = (ECardValue)cardValue,
                    CardColor = (ECardColor)cardColor,
                });
                playingCards.Add(new GameCard()
                {
                    CardValue = (ECardValue)cardValue,
                    CardColor = (ECardColor)cardColor,
                });
                playingCards.Add(new GameCard()
                {
                    CardValue = ECardValue.Value0,
                    CardColor = (ECardColor)cardColor,
                });
            }
        }
        for (int cardValue = (int)ECardValue.Add4; cardValue <= (int)ECardValue.Blank; cardValue++)
        {
            playingCards.Add(new GameCard()
            {
                CardValue = (ECardValue)cardValue,
                CardColor = ECardColor.Black
            });
        }
        
        while (playingCards.Count > 0)
        {
            var randomPositionInDeck = Rnd.Next(playingCards.Count);
            State.CardsNotInPlay.Enqueue(playingCards[randomPositionInDeck]);
            playingCards.RemoveAt(randomPositionInDeck);
        }
        

        foreach (var player in State.Players)
        {
            for (int cardRandom = 0; cardRandom < 7; cardRandom++)
            {
                player.PlayerHand.Add(State.CardsNotInPlay.Dequeue());
            }
        }
        State.CardToBeat = State.CardsNotInPlay.Dequeue();
    }

    public bool IsMoveValid(GameCard card)
    {
        if (card.CardColor == ECardColor.Black)
        {
            return true;
        }
        return State.CardToBeat!.CardColor == card.CardColor || State.CardToBeat!.CardValue == card.CardValue;
    }
    
    public bool IsGameOver()
    {
        return false;
    }
    
}