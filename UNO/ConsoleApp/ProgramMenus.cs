using DAL;
using Domain;
using MenuSystem;
using UnoEngine;

namespace ConsoleApp;

public static class ProgramMenus
{
    public static Menu GetOptionsMenu(Rules rules, EMenuLevel menuLevel = EMenuLevel.Sub1) =>
        new Menu("Configure rules", new List<MenuItem>()
        {
            new MenuItem()
            {
                Hotkey = "h",
                MenuLabelFunction = () => "Hand size - " + rules.HandSize,
                MethodToRun = () => RulesSetup.ConfigureHandSize(rules)
            },
            new MenuItem()
            {
                Hotkey = "a",
                MenuLabelFunction = () =>
                    "Cards added to players hand if no move available - " + rules.CardAddition,
                MethodToRun = () => RulesSetup.ConfigureCardAdd(rules)
            },
            new MenuItem()
            {
                Hotkey = "m",
                MenuLabelFunction = () =>
                    "Allow multiple card moves - " + (rules.MultipleCardMoves ? "yes" : "no"),
                MethodToRun = () => RulesSetup.ConfigureMultipleCardMoves(rules)
            },
        }, menuLevel);

    public static Menu GetLoadGameMenu(IGameRepository gameRepository, Func<Guid, string?> loadGameMethod,
        EMenuLevel menuLevel = EMenuLevel.Sub1)
    {
        var saveGameList = gameRepository.GetSaveGames();

        if (saveGameList.Count == 0)
        {
            return new Menu("No save files found", new List<MenuItem>(), menuLevel);
        }

        int i = 1;
        var menuList = new List<MenuItem>();
        foreach (var save in saveGameList)
        {
            var menuItem = new MenuItem()
            {
                Hotkey = i.ToString(),
                MenuLabelFunction = () => " " + save.dt,
                MethodToRun = () => loadGameMethod(save.id)
            };
            menuList.Add(menuItem);
            i++;
        }

        var menu = new Menu("Saved games", menuList, menuLevel);
        return menu;
    }

    public static Menu GetMainMenu(Func<string?> newGameMethod, Func<Menu> loadGameMenuMethod, Menu rulesMenu,
        Rules rules)
    {
        Menu menu = new Menu("<<< ||U||N||O|| >>>", new List<MenuItem>()
        {
            new MenuItem()
            {
                Hotkey = "s",
                Label = " Start a new game: ",
                MenuLabelFunction = () => " Start a new game:\nHand size - " + rules.HandSize +
                                          "\nCards added to players hand if no move available - " +
                                          rules.CardAddition +
                                          "\nAllow multiple card moves - " +
                                          (rules.MultipleCardMoves ? "yes" : "no"),
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
                MethodToRun = () => loadGameMenuMethod().Run()
            },
        });
        return menu;
    }
}


