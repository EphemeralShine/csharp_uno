using UIConsole;
using UnoEngine;

namespace ConsoleApp;

public static class RulesSetup
{
    public static string? ConfigureHandSize(GameEngine gameEngine)
    {
        gameEngine.State.GameRules.HandSize =
            Prompts.PromptWithDefault("Player hand size (5 - 19), default 7:", "^(1[0-9]|[2-9])$", 7);
        return null;
    }

    public static string? ConfigureCardAdd(GameEngine gameEngine)
    {
        gameEngine.State.GameRules.CardAddition =
            Prompts.PromptWithDefault(
                "Cards added to players deck if there is no move for the player (1-4), default 2:", "^[1-7]$", 2);
        return null;
    }

    public static string? ConfigureMultipleCardMoves(GameEngine gameEngine)
    {
        var multipleCardMoves =
            Prompts.PromptWithDefault("Allow multiple card inputs (Y/N):", "^[YNyn]$", "y").ToLower();
        if (multipleCardMoves == "n")
        {
            gameEngine.State.GameRules.MultipleCardMoves = false;
        }

        return null;
    }
}