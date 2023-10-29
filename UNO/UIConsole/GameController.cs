using System.Data;
using System.Text.RegularExpressions;
using Domain;
using UnoEngine;

namespace UIConsole;

public class GameController
{
    private readonly GameEngine _gameEngine;
    
    public GameController(GameEngine gameEngine)
    {
        _gameEngine = gameEngine;
    }

    private bool ValidateInput(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }
        string pattern = "^[0-9,]+$";
        return Regex.IsMatch(input.Trim(), pattern);
    }

    public void GameLoop()
    {
        Console.Clear();
        //1.Show first card to beat
        Console.WriteLine($"Starting card is: {_gameEngine.State.CardToBeat}");
        //2.Perform actions tied with it

        _gameEngine.CardsAction(_gameEngine.State.CardToBeat!);
        while (_gameEngine.IsGameOver() == false)//game over loop
        {
            //Empty Movelist
            List<GameCard> moveList = new();
            //Ask player if he is ready to see his deck
            Console.WriteLine($"Player {_gameEngine.State.ActivePlayerNo + 1} - {_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].Name}");
            Console.Write("Your turn, make sure you are alone looking at screen! Press enter to continue...");
            Console.ReadLine();
            
            while (true)//player move loop
            {
                while (true)//loop to check if player has cards to play
                {
                    Console.Clear();
                    //2.Show player deck
                    Console.WriteLine(
                        $"Player {_gameEngine.State.ActivePlayerNo + 1} - {_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].Name}");
                    ConsoleVisualization.DrawDesk(_gameEngine.State);
                    ConsoleVisualization.DrawPlayerHand(_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo]);
                    //If no cards to play, inform player and wait for input to add 2 card to player hand and get back to step 2
                    if (_gameEngine.IsPlayerAbleToMove() == false)
                    {
                        Console.WriteLine("No suitable cards in your Deck! Adding 2");
                        _gameEngine.Add2CardsToPlayer();
                        Console.WriteLine("Press enter to proceed...");
                        Console.ReadLine();
                    }
                    else
                    {
                        break;
                    }
                }
                //Ask for card input
                var playerChoiceStr = "";
                while (true)//Input validation loop
                {
                    Console.Write(
                        $"Choose card(s) to play (first input card being the next card to beat) 1-{_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].PlayerHand.Count}, comma separated:");
                    playerChoiceStr = Console.ReadLine();
                    //Validate input
                    if (ValidateInput(playerChoiceStr) == false)
                    {
                        Console.WriteLine("Wrong input style (correct input: 1,2,3)");
                    }
                    else
                    {
                        break;
                    }
                }
                var playerChoices = playerChoiceStr!.Split(",").Select(s => int.Parse(s.Trim()));
                //Add cards to moveList
                foreach (var num in playerChoices)
                {
                    if (num > _gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].PlayerHand.Count || num < 1)
                    {
                        Console.WriteLine("No card with such index in your deck!");
                        continue;
                    }
                    moveList.Add(_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].PlayerHand[num - 1]);
                }
                //Validate move legality
                if (_gameEngine.IsMoveValid(moveList) == false)
                {
                    Console.WriteLine("Illegal move!");
                    continue;
                }
                
                //Update player deck
                _gameEngine.UpdatePlayerHand(moveList);
                //Perform card actions
                foreach (var card in moveList)
                {
                    _gameEngine.CardsAction(card);
                }
                //Update card to beat, move the old one and other cards in move to q 
                _gameEngine.UpdateCardToBeat(moveList);
                //Update player
                _gameEngine.UpdateActivePlayerNo();
                break;
            }
            
        }
    }
}