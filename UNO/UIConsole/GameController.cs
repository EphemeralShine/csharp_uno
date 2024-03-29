﻿using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using DAL;
using Domain;
using UnoEngine;

namespace UIConsole;

public class GameController
{
    private readonly GameEngine _gameEngine;
    private readonly IGameRepository _repository;
    
    public GameController(GameEngine gameEngine, IGameRepository repo)
    {
        _gameEngine = gameEngine;
        _repository = repo;
    }
    
    public string GameLoop()
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.Clear();
        //1.Show first card to beat
        Console.WriteLine($"Starting card is: {_gameEngine.State.CardToBeat}");
        if (_gameEngine.State.LastMove != null && _gameEngine.State.LastMove.Count != 0)
        {
            ConsoleVisualization.DrawPreviousMove(_gameEngine.State);
        }
        while (_gameEngine.IsGameOver() == false) //game over loop
        {
            _gameEngine.DetermineWinner();
            if (_gameEngine.State.LastMove != null && _gameEngine.State.LastMove.Count != 0)
            {
                ConsoleVisualization.DrawPreviousMove(_gameEngine.State);
            }
            //Empty Movelist
            List<GameCard>? moveList = new();
            //Ask player if he is ready to see his deck
            Console.WriteLine(
                "Game progress is saved after each move automatically. Press 'x' to go back to main menu");
            if (_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].PlayerType == EPlayerType.Ai)
            {
                Console.WriteLine(
                    $"Player (controlled by AI) {_gameEngine.State.ActivePlayerNo + 1} - {_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].Name}");
                Console.Write("Press any button to let AI make a move...");
            }
            else
            {
                Console.WriteLine(
                    $"Player {_gameEngine.State.ActivePlayerNo + 1} - {_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].Name}");
                Console.Write("Your turn, make sure you are alone looking at screen! Press any button to continue...");
            }

            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.X)
            {
                Console.WriteLine("\n");
                return "r";
            }

            if (_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].PlayerType == EPlayerType.Ai)
            {
                while (true)
                {
                    moveList = _gameEngine.AIMove();
                    if (moveList == null)
                    {
                        Console.WriteLine(
                            $"Player (controlled by AI) {_gameEngine.State.ActivePlayerNo + 1} - {_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].Name} has no suitable cards. Adding 2...");
                        _gameEngine.AddCardsToPlayer();
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        _gameEngine.UpdatePlayerHand(moveList);
                        _gameEngine.CardsAction(moveList);
                        _gameEngine.UpdateCardToBeat(moveList);
                        _gameEngine.UpdateActivePlayerNo();
                        _gameEngine.State.LastMove = moveList;
                        _repository.Save(_gameEngine.State.Id, _gameEngine.State);                    
                        break;
                    }
                }
            }
            else
            {
                while (true) //player move loop
                {
                    while (true) //loop to check if player has cards to play
                    {
                        //2.Show player deck
                        Console.WriteLine(
                            $"Player {_gameEngine.State.ActivePlayerNo + 1} - {_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].Name}");
                        ConsoleVisualization.DrawDesk(_gameEngine.State);
                        ConsoleVisualization.DrawPlayerHand(
                            _gameEngine.State.Players[_gameEngine.State.ActivePlayerNo]);
                        //If no cards to play, inform player and wait for input to add 2 card to player hand and get back to step 2
                        if (_gameEngine.IsPlayerAbleToMove() == false)
                        {
                            Console.WriteLine(
                                $"No suitable cards in your Deck! Adding {_gameEngine.State.GameRules.CardAddition}");
                            _gameEngine.AddCardsToPlayer();
                            Console.WriteLine("Press enter to proceed...");
                            Console.ReadKey();
                        }
                        else
                        {
                            break;
                        }
                    }

                    //Ask for card input
                    while (true)
                    {
                        bool flag = false;
                        var cardPromptString =
                            $"Choose card(s) to play (first card beating current card to beat, last card being the next card to beat) 1-{_gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].PlayerHand.Count}, comma separated:)";
                        string playerChoiceStr = Prompts.Prompt<string>(cardPromptString, "^[0-9,]+$");
                        var playerChoices = playerChoiceStr!.Split(",").Select(s => int.Parse(s.Trim()));
                        //Add cards to moveList
                        foreach (var num in playerChoices)
                        {

                            if (num > _gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].PlayerHand.Count ||
                                num < 1)
                            {
                                Console.WriteLine("No card with such index in your deck!");
                                flag = true;
                                break;
                            }

                            moveList.Add(
                                _gameEngine.State.Players[_gameEngine.State.ActivePlayerNo].PlayerHand[num - 1]);
                        }

                        if (flag)
                        {
                            continue;
                        }

                        break;
                    }

                    //Validate move legality
                    if (_gameEngine.IsMoveValid(moveList) == false)
                    {
                        Console.WriteLine("Illegal move!");
                        moveList.Clear();
                        continue;
                    }

                    //Update player deck
                    _gameEngine.UpdatePlayerHand(moveList);
                    //Perform card actions
                    if (moveList[0].CardColor == ECardColor.Black)
                    {
                        ECardColor playerColorChange = ECardColor.None;
                        var playerColorStr = Prompts.Prompt<string>("Choose next color 1-4: 1)🔴, 2)🔵, 3)🟢, 4)🟡",
                            "^(1|2|3|4)$");
                        playerColorChange = playerColorStr switch
                        {
                            "1" => ECardColor.Red,
                            "2" => ECardColor.Blue,
                            "3" => ECardColor.Green,
                            "4" => ECardColor.Yellow,
                            _ => playerColorChange
                        };
                        _gameEngine.CardsAction(moveList, playerColorChange);
                    }
                    else
                    {
                        _gameEngine.CardsAction(moveList);
                    }

                    //Update card to beat, move the old one and other cards in move to q 
                    _gameEngine.UpdateCardToBeat(moveList);
                    //Update player
                    _gameEngine.UpdateActivePlayerNo();
                    _repository.Save(_gameEngine.State.Id, _gameEngine.State);
                    _gameEngine.State.LastMove = moveList;
                    break;
                }

            }
        }

        _gameEngine.DetermineWinner();
        _gameEngine.DetermineLoser();
        Console.Clear();
        Console.WriteLine("<<<>>> GAME OVER <<<>>> GAME OVER <<<>>> GAME OVER <<<>>>");
        if (_gameEngine.State.Placings.Count > 0)
        {
            Console.WriteLine($"The Winner is: {_gameEngine.State.Placings[0].Name}");
        }
        if (_gameEngine.State.Placings.Count > 0) 
        {
            Console.WriteLine($"The loser is: {_gameEngine.State.Placings[1].Name}");
        }
        Console.WriteLine("Press enter to proceed:");
        Console.ReadLine();
        Console.WriteLine("\n");
        return "r";
    }
}