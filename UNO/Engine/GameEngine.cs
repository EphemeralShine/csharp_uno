﻿using Domain;

namespace UnoEngine;

public class GameEngine
{
    private Random Rnd { get; set; } = new Random();
    public GameState State { get; set; } = new GameState();
    
    public void InitializeGameStartingState()
    {
        var playingCards = new List<GameCard>();
        for (int cardValue = 1; cardValue < (int)ECardValue.Add4; cardValue++)
        {
            for (int cardColor = 0; cardColor < (int)ECardColor.Black; cardColor++)
            { 
                for (int i = 0; i < 2; i++)
                {
                    playingCards.Add(new GameCard()
                    {
                        CardValue = (ECardValue)cardValue,
                        CardColor = (ECardColor)cardColor,
                    });
                }

            }
        }

        for (int cardColor = 0; cardColor < (int)ECardColor.Black; cardColor++)
        {
            playingCards.Add(new GameCard()
            {
                CardValue = ECardValue.Value0,
                CardColor = (ECardColor)cardColor,
            });
        }


        for (int cardValue = (int)ECardValue.Add4; cardValue < (int)ECardValue.Blank; cardValue++)
        {
            for (int i = 0; i < 4; i++)
            {
                playingCards.Add(new GameCard()
                {
                    CardValue = (ECardValue)cardValue,
                    CardColor = ECardColor.Black
                });
            }
        }

        while (playingCards.Count > 0)
        {
            var randomPositionInDeck = Rnd.Next(playingCards.Count);
            State.CardsNotInPlay.Enqueue(playingCards[randomPositionInDeck]);
            playingCards.RemoveAt(randomPositionInDeck);
        }


        foreach (var player in State.Players)
        {
            for (int cardRandom = 0; cardRandom < State.GameRules.HandSize; cardRandom++)
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
        State.ActivePlayerNo = 0;
    }

    public bool IsMoveValid(List<GameCard> cards)
    {
        if (!State.GameRules.MultipleCardMoves && cards.Count != 1)
        {
            return false;
        }
        
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

    public void CardsAction(List<GameCard> cards, ECardColor colorChange = ECardColor.None)
    {
        if (cards[0].CardValue == ECardValue.Skip)
        { 
            int selfSkipCount = cards.Count / State.Players.Count;
            foreach (var card in cards)
            {
                State.ActivePlayerNo = GetNextPlayerNo();
            }
            for (int i = 0; i < selfSkipCount; i++)
            {
                State.ActivePlayerNo = GetNextPlayerNo(); 
            }
            return;
        }
        foreach (var card in cards)
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

            if (card.CardValue == ECardValue.ChangeColor && colorChange != ECardColor.None)
            {
                State.CurrentColor = colorChange;
            }
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
                if (nextPlayerNo > State.Players.Count - 1)
                {
                    nextPlayerNo = 0;
                }
            } while (State.Players[nextPlayerNo].PlayerHand.Count == 0);
        }

        if (State.ClockwiseMoveOrder == false)
        {
            do
            {
                nextPlayerNo--;
                if (nextPlayerNo < 0)
                {
                    nextPlayerNo = State.Players.Count - 1;
                }
            } while (State.Players[nextPlayerNo].PlayerHand.Count == 0);

        }

        return nextPlayerNo;
    }

    public void UpdatePlayerHand(List<GameCard> cards)
    {
        foreach (var card in cards)
        {
            State.Players[State.ActivePlayerNo].PlayerHand.Remove(card);
        }
    }

    public void UpdateCardToBeat(List<GameCard> cards)
    {
        State.CardsNotInPlay.Enqueue(State.CardToBeat!);
        for (int card = 0; card < cards.Count - 1; card++)
        {
            State.CardsNotInPlay.Enqueue(cards[card]);
        }

        State.CardToBeat = cards[^1];
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
            if (card.CardColor == ECardColor.Black)
            {
                return true;
            }

            if (card.CardValue == State.CardToBeat!.CardValue || card.CardColor == State.CurrentColor)
            {
                return true;
            }
        }

        return false;
    }

    public void AddCardsToPlayer()
    {
        for (int j = 0; j < State.GameRules.CardAddition; j++)
        {
            State.Players[State.ActivePlayerNo].PlayerHand.Add(State.CardsNotInPlay.Dequeue());
        }
    }
    
    /*public Player? DetermineLoser()
    {
        foreach (var player in State.Players)
        {
            if (player.PlayerHand.Count > 0)
            {
                return player;
            }
        }
        return null;
    }*/

    public void Placings()
    {
        if (State.Placings.Count >= 3) return;
        foreach (var player in State.Players)
        {
            if (player.PlayerHand.Count < 0)
            {
                State.Placings.Enqueue(player);
            }
        }
    }

    public int DetermineMaxPlayerCount()
    {
        var availableCards = 107;
        return availableCards / State.GameRules.HandSize;
    }
    
    
    //AI stuff
    public List<GameCard>? AIMove()
    {
        var move = new List<GameCard>();
        var desiredMoveCards = new List<GameCard>();
        var validMoveCards = new List<GameCard>();
        var hand = new List<GameCard>(State.Players[State.ActivePlayerNo].PlayerHand);
        // determine valid cards for the bottom card in the move
        foreach (var card in hand)
        {
            if (card.CardColor == ECardColor.Black || card.CardColor == State.CurrentColor ||
                card.CardValue == State.CardToBeat!.CardValue)
            {
                validMoveCards.Add(card);
            }
        }

        foreach (var card in validMoveCards)
        {
            hand.Remove(card);
        }
        // controller handles if no moves available ( to add some visualisation )
        if (validMoveCards.Count == 0)
        {
            return null;
        }
        else
        {
            // if next player has less than 2 cards prioritize +cards cards
            if (State.Players[GetNextPlayerNo()].PlayerHand.Count <= 2)
            {
                foreach (var card in validMoveCards)
                {
                    if (card.CardValue is ECardValue.Add2 or ECardValue.Add4)
                    {
                        desiredMoveCards.Add(card);
                    }
                }
            }
            // if there are no priority moves choose randomly
            if (desiredMoveCards.Count == 0)
            {
                if (!State.GameRules.MultipleCardMoves)
                {
                    move.Add(validMoveCards[Rnd.Next(validMoveCards.Count)]);
                }
                else
                {
                    // if multiple card moves are enabled, choose random group, where there are the most amount of cards
                    // valid move cards are expanded because second move card can be matching value with first card,
                    // but different value and color for the card to beat
                    var cardsToAdd = new List<GameCard>();
                    foreach (var cardX in validMoveCards)
                    {
                        foreach (var cardY in hand)
                        {
                            if (cardX.CardValue == cardY.CardValue)
                            {
                                cardsToAdd.Add(cardY);
                            }
                        }
                        foreach (var card in cardsToAdd)
                        {
                            hand.Remove(card);
                        }
                    }
                    foreach (var card in cardsToAdd)
                    {
                        validMoveCards.Add(card);
                    }
                    move = AIMaxGroup(validMoveCards);
                    // NB! make sure first move card is valid ( matching color/value of card to beat)
                    move = move.OrderBy(card => card.CardColor == State.CurrentColor ? 0 : 1).ToList();
                }
            }
            else
            {
                if (!State.GameRules.MultipleCardMoves)
                {
                    move.Add(desiredMoveCards[Rnd.Next(desiredMoveCards.Count)]);
                }
                else
                {
                    foreach (var cardX in desiredMoveCards)
                    {
                        var cardsToRemove = new List<GameCard>();
                        foreach (var cardY in hand)
                        {
                            if (cardX.CardValue == cardY.CardValue)
                            {
                                desiredMoveCards.Add(cardY);
                                cardsToRemove.Add(cardY);
                            }
                        }
                        foreach (var card in cardsToRemove)
                        {
                            hand.Remove(card);
                        }
                    }
                    move = AIMaxGroup(desiredMoveCards);
                    move = move.OrderBy(card => card.CardColor == State.CurrentColor ? 0 : 1).ToList();
                }
            }
        }
        // if move contains color changes, handle them
        if (move.Any(card => card.CardColor == ECardColor.Black))
        {
            // choose color that player has the most colors of
            State.CurrentColor = AIColor(State.Players[State.ActivePlayerNo].PlayerHand);
        }
        return move;
    }

    private ECardColor AIColor(List<GameCard> hand)
    {
        var moveGroupsColor = hand.GroupBy(card => card.CardColor);
        var largestGroupColor = moveGroupsColor.OrderByDescending(group => group.Count()).First();
        return largestGroupColor.Key;
    }

    private List<GameCard> AIMaxGroup(List<GameCard> validMoveCards)
    {
        var move = new List<GameCard>();
        var moveGroupsValue = validMoveCards.GroupBy(card => card.CardValue);
        var largestGroupValue = moveGroupsValue.OrderByDescending(group => group.Count()).First();
        foreach (var card in largestGroupValue)
        {
            move.Add(card);
        }
        return move;
    }
}



