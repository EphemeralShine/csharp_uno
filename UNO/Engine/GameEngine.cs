using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Domain;

namespace UnoEngine;

public class GameEngine
{
    private Random Rnd { get; set; } = new Random();
    public GameState State { get; set; } = new GameState();
    
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

        while (true)
        {
            State.CardToBeat = State.CardsNotInPlay.Dequeue();
            if (State.CardToBeat.CardColor == ECardColor.Black)
            {
                State.CardsNotInPlay.Enqueue(State.CardToBeat);
            }
            else
            { 
                break;
            }
        }

        State.CurrentColor = State.CardToBeat.CardColor;
        State.ActivePlayerNo = 0; //TODO: how to choose a starter
    }

    public bool IsMoveValid(List<GameCard> cards)
    {
        switch (cards.Count)
        {
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

    public void CardsAction(GameCard card, ECardColor colorChange = ECardColor.None)
    {
        if (card.CardValue == ECardValue.Add2)
        {
                for (int j = 0; j < 2; j++)
                {
                    State.Players[GetNextPlayerNo()].PlayerHand.Add(State.CardsNotInPlay.Dequeue());
                }
        }

        if (card.CardValue == ECardValue.Add4)
        {
                for (int j = 0; j < 4; j++)
                {
                    State.Players[GetNextPlayerNo()].PlayerHand.Add(State.CardsNotInPlay.Dequeue());
                }

                if (colorChange != ECardColor.None)
                {
                    State.CurrentColor = colorChange;
                }
        }

        if (card.CardValue == ECardValue.Reverse)
        {
                State.ClockwiseMoveOrder = !State.ClockwiseMoveOrder;
        }
        
        if (card.CardValue == ECardValue.Skip)
        {
            State.ActivePlayerNo = GetNextPlayerNo();
        }
        
        if (card.CardValue == ECardValue.ChangeColor && colorChange != ECardColor.None)
        {
            State.CurrentColor = colorChange;
        }
    }

    private int GetNextPlayerNo()
    {
        int nextPlayerNo = State.ActivePlayerNo;
        if (State.ClockwiseMoveOrder)
        {
            do
            {
                nextPlayerNo++;
                if (nextPlayerNo > State.Players.Count)
                {
                    nextPlayerNo = 0;
                }
            }while(State.Players[nextPlayerNo].PlayerHand.Count == 0);
        }
        else
        {
            do
            {
                nextPlayerNo--;
                if (nextPlayerNo < 0)
                {
                    nextPlayerNo = State.Players.Count;
                }
            }while(State.Players[nextPlayerNo].PlayerHand.Count == 0);
            
        }
        return nextPlayerNo;
    }
    public void UpdatePlayerHand(List<GameCard> cards)
    {
        foreach(var card in cards)
        {
            State.Players[State.ActivePlayerNo].PlayerHand.Remove(card);
        }
    }

    public void UpdateCardToBeat(List<GameCard> cards)
    {
        State.CardsNotInPlay.Enqueue(State.CardToBeat!);
        for (int card = 1; card < cards.Count; card++)
        {
            State.CardsNotInPlay.Enqueue(cards[card]);
        }
        State.CardToBeat = cards[0];
        if (State.CardToBeat.CardColor != ECardColor.Black)
        {
            State.CurrentColor = State.CardToBeat.CardColor;
        }
    }

    public void UpdateActivePlayerNo()
    {
        State.ActivePlayerNo = GetNextPlayerNo();
    }

    public bool IsPlayerAbleToMove()
    {
        foreach (var card in State.Players[State.ActivePlayerNo].PlayerHand)
        {
            if (card.CardValue == State.CardToBeat!.CardValue || card.CardColor == State.CurrentColor)
            {
                return true;
            }
        }
        return false;
    }

    public void Add2CardsToPlayer()
    {
        for (int j = 0; j < 2; j++)
        {
            State.Players[State.ActivePlayerNo].PlayerHand.Add(State.CardsNotInPlay.Dequeue());
        }
    }
}