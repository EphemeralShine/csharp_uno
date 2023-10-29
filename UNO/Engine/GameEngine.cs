using System.Runtime.InteropServices;
using Domain;

namespace UnoEngine;

public class GameEngine
{
    private Random Rnd { get; set; } = new Random();
    public GameState State { get; set; } = new GameState();

    public GameEngine()
    {
        InitializeGameStartingState();
    }
    
    private void InitializeGameStartingState()
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
        for (int cardValue = (int)ECardValue.Add4; cardValue < (int)ECardValue.Blank; cardValue++)
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
        State.CurrentColor = State.CardToBeat.CardColor;
        State.ActivePlayerNo = 0; //TODO: how to choose a starter
    }

    public bool IsMoveValid(List<GameCard> cards)
    {
        switch (cards.Count)
        {
            case 0:
                return true;
            case 1 when cards[0].CardColor == ECardColor.Black:
                return true;
            case 1:
                return State.CurrentColor == cards[0].CardColor || State.CardToBeat!.CardValue == cards[0].CardValue;
            case > 1 when cards.All(card => card.CardValue == cards[0].CardValue):
            {
                if (cards[0].CardColor == ECardColor.Black)
                {
                    return true;
                }

                return State.CurrentColor == cards[0].CardColor || State.CardToBeat!.CardValue == cards[0].CardValue;
            }
            case > 1:
                return false;
        }
        return false;
    }
    
    public bool IsGameOver()
    {
        var count = 0;
        foreach (var player in State.Players)
        {
            if (player.PlayerHand.Count > 0)
            {
                count += 1;
            }
        }
        return count <= 1;
    }

    public void CardsAction(List<GameCard> cards, ECardColor colorChange = ECardColor.None)
    {
        var count = cards.Count;
        if (cards[0].CardValue == ECardValue.Add2)
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    State.Players[State.ActivePlayerNo + 1].PlayerHand.Add(State.CardsNotInPlay.Dequeue());
                }//TODO: next player fix
            }
        }

        if (cards[0].CardValue == ECardValue.Add4)
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    State.Players[State.ActivePlayerNo + 1].PlayerHand.Add(State.CardsNotInPlay.Dequeue());
                }//TODO: next player fix
            }

            if (colorChange != ECardColor.None)
            {
                State.CurrentColor = colorChange;
            }
        }

        if (cards[0].CardValue == ECardValue.Reverse)
        {
            for (int i = 0; i < count; i++)
            {
                State.ClockwiseMoveOrder = !State.ClockwiseMoveOrder;
            }
        }
        
        if (cards[0].CardValue == ECardValue.Skip)
        {
            for (int i = 0; i < count; i++)
            {
                State.ActivePlayerNo += 1;
                //TODO: next player fix
            }
        }
        
        if (cards[0].CardValue == ECardValue.ChangeColor && colorChange != ECardColor.None)
        {
            State.CurrentColor = colorChange;
        }
    }
    
    public void UpdateState()
    {
        throw new NotImplementedException();
    }
    public void ElectNextPlayer()
    {
        throw new NotImplementedException();
    }
}