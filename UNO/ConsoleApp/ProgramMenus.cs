using Domain;
using MenuSystem;

namespace ConsoleApp;

public class ProgramMenus
{
    /*public static Menu GetOptionsMenu(Rules rules) =>
        new Menu("Options", new List<MenuItem>()
        {
            new MenuItem()
            {
                Hotkey = "h",
                MenuLabelFunction = () => "Hand size - " + rules.HandSize,
                MethodToRun = () => OptionsChanger.ConfigureHandSize(rules)
            },
            new MenuItem()
            {
                Hotkey = "s",
                MenuLabelFunction = () => "Use small cards - " + (rules.UseSmallCards ? "yes" : "no"),
                MethodToRun = () => OptionsChanger.ConfigureSmallCards(rules)
            },
        });*/


    public static Menu GetMainMenu(Func<string?> newGameMethod, Func<string?> loadGameMethod)
    {
        Menu menu = new Menu ("<<< ||U||N||O|| >>>", new List<MenuItem>()
        {
            new MenuItem()
            {
                Hotkey = "s",
                Label = "Start a new game: ",
                MenuLabelFunction = () => "Start a new game: "/* + rules*/,
                MethodToRun = newGameMethod
            },
            new MenuItem()
            {
                Hotkey = "l",
                Label = "Load game",
                MethodToRun = loadGameMethod
            },/*
            new MenuItem()
            {
                Hotkey = "o",
                Label = "Options",
                MethodToRun = optionsMenu.Run
            },*/
        });
        return menu;
    }
}