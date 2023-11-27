using Domain;
using UIConsole;
using UnoEngine;

namespace ConsoleApp;

public static class RulesSetup
{
    public static string? ConfigureHandSize(Rules rules)
    {
        rules.HandSize =
            Prompts.PromptWithDefault("Player hand size (5 - 19), default 7:", "^(1[0-9]|[2-9])$", 7);
        return null;
    }

    public static string? ConfigureCardAdd(Rules rules)
    {
        rules.CardAddition =
            Prompts.PromptWithDefault(
                "Cards added to players deck if there is no move for the player (1-4), default 2:", "^[1-4]$", 2);
        return null;
    }

    public static string? ConfigureMultipleCardMoves(Rules rules)
    {
        var multipleCardMoves =
            Prompts.PromptWithDefault("Allow multiple card inputs (Y/N):", "^[YNyn]$", "y").ToLower();
        if (multipleCardMoves == "n")
        {
            rules.MultipleCardMoves = false;
        }

        return null;
    }
}