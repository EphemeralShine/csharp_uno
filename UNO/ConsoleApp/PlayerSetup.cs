using Domain;
using UIConsole;
using UnoEngine;

namespace ConsoleApp;

public static class PlayerSetup
{
    public static void ConfigurePlayers(GameEngine gameEngine)
    {
        var maxPlayers = gameEngine.DetermineMaxPlayerCount();
        string playerCountRegex;
        switch (maxPlayers)
        {
            case < 10:
                playerCountRegex = $"^[2-9]$";
                break;
            case >= 10 and < 20:
                playerCountRegex = $"^([2-9]|1[0-{maxPlayers - 10}])$";
                break;
            case >= 20:
                playerCountRegex = $"^([2-9]|1[0-9]|2[0-{maxPlayers - 20}])$";
                break;
        }

        // get player amount
        var playerCount = Prompts.PromptWithDefault($"How many players (2 - {maxPlayers}):", playerCountRegex, 2);
        
        // configure players
        for (int i = 0; i < playerCount; i++)
        {
            // player type
            var typePrompt = $"Player {i + 1} type (A - ai / H - human):";
            var playerType = Prompts.PromptWithDefault(typePrompt, "^[AHah]$", "a").ToLower();

            // player name
            var namePrompt = $"Player {i + 1} name (min 1 letter, max 16)[{playerType + (i + 1)}]:";
            var defaultName = playerType + (i + 1);
            var playerName = Prompts.PromptWithDefault(namePrompt, "^.{1,16}$", defaultName);
            
            // set values
            gameEngine.State.Players.Add(new Player()
            {
                Name = playerName,
                PlayerType = playerType == "h" ? EPlayerType.Human : EPlayerType.Ai
            });

        }

    }
}