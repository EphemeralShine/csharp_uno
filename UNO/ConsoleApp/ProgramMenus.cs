using Domain;
using MenuSystem;
using UnoEngine;

namespace ConsoleApp;

public static class ProgramMenus
{
    public static Menu GetOptionsMenu(GameEngine gameEngine, EMenuLevel menuLevel = EMenuLevel.Sub1) =>
        new Menu("Configure rules", new List<MenuItem>()
        {
            new MenuItem()
            {
                Hotkey = "h",
                MenuLabelFunction = () => "Hand size - " + gameEngine.State.GameRules.HandSize,
                MethodToRun = () => RulesSetup.ConfigureHandSize(gameEngine)
            },
            new MenuItem()
            {
                Hotkey = "a",
                MenuLabelFunction = () => "Cards added to players hand if no move available - " + gameEngine.State.GameRules.CardAddition,
                MethodToRun = () => RulesSetup.ConfigureCardAdd(gameEngine)
            },
            new MenuItem()
            {
                Hotkey = "m",
                MenuLabelFunction = () => "Allow multiple card moves - " + (gameEngine.State.GameRules.MultipleCardMoves ? "yes" : "no"),
                MethodToRun = () => RulesSetup.ConfigureMultipleCardMoves(gameEngine)
            },
        }, menuLevel);


    public static Menu GetMainMenu(Func<string?> newGameMethod, Func<string?> loadGameMethod, Menu rulesMenu, GameEngine gameEngine)
    {
        Menu menu = new Menu ("<<< ||U||N||O|| >>>", new List<MenuItem>()
        {
            new MenuItem()
            {
                Hotkey = "s",
                Label = " Start a new game: ",
                MenuLabelFunction = () => " Start a new game:\nHand size - " + gameEngine.State.GameRules.HandSize + 
                                          "\nCards added to players hand if no move available - " + gameEngine.State.GameRules.CardAddition + 
                                          "\nAllow multiple card moves - " + (gameEngine.State.GameRules.MultipleCardMoves ? "yes" : "no"),
                MethodToRun = newGameMethod
            },
            new MenuItem()
            {
                Hotkey = "c",
                Label = " Configure rules for a new game",
                MethodToRun = rulesMenu.Run
            },
            new MenuItem()
            {
                Hotkey = "l",
                Label = " Load game",
                MethodToRun = loadGameMethod
            },
        });
        return menu;
    }
}

