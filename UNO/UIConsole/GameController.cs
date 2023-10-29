using System.Data;
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

    public void GameLoop()
    {
        while (_gameEngine.IsGameOver() == false)
        {
            List<GameCard> bufferList = new();
            //1.Show card to beat
            Console.WriteLine("Card to beat" + _gameEngine.State.CardToBeat);
            //2.Perform actions tied with it

            _gameEngine.CardsAction(_gameEngine.State.CardToBeat);
            //3.Update active player
            //4.Show player deck, ask for card input
            //5.Validate input, go back to step 3 until right input
            //6.If no input, add 2 cards to player, go back to step 3 
            //7.Validate move legality, illegal move - go back to step 3
            //8.Update player deck
            //9.Move beaten card to queue CardsNotInPlay (maybe implement buffer, so can undo the move)
            //10.Update card to beat
            
        }
    }
}