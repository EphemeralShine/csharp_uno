using System.Security.Principal;

namespace MenuSystem;

public class Menu
{
    public EMenuLevel MenuLevel { get; set; }
    public string? Title { get; set; }
    public Dictionary<string, MenuItem> MenuItems { get; set; } = new();
    private const string MenuSeparator = "======================";
    private static readonly HashSet<string> ReservedHotkeys = new() {"x", "b", "r"};

    public Menu(string? title, List<MenuItem> menuItems, EMenuLevel menuLevel = EMenuLevel.Main)
    {
        MenuLevel = menuLevel;
        Title = title;
        foreach (var menuItem in menuItems)
        {
            if (ReservedHotkeys.Contains(menuItem.Hotkey.ToLower()))
            {
                throw new ApplicationException(
                    $"Menu hotkey '{menuItem.Hotkey}'is reserved!");
            }
            
            if (MenuItems.ContainsKey(menuItem.Hotkey.ToLower()))
            {
                throw new ApplicationException(
                    $"Hotkey '{menuItem.Hotkey}' already in use!");
            }

            MenuItems[menuItem.Hotkey.ToLower()] = menuItem;
        }
    }
    
    private void Draw()
    {
        if (!string.IsNullOrWhiteSpace(Title))
        {
            Console.WriteLine(Title);
            Console.WriteLine(MenuSeparator);
        }

        foreach (var menuItem in MenuItems)
        {
            Console.Write($"{menuItem.Key})");
            Console.WriteLine(menuItem.Value.Label);
        }

        if (MenuLevel == EMenuLevel.Sub2)
        {
            Console.WriteLine("b) Back");
        }

        if (MenuLevel != EMenuLevel.Main)
        {
            Console.WriteLine("r) Return to main");
        }
        Console.WriteLine("x) eXit");

        Console.WriteLine(MenuSeparator);
        Console.Write("Your choice:");

    }

    public string Run()
    {
        /*while (true)
        {*/
        //TODO: Main menu no back available
        var userChoice = "";
        Console.Clear();
            while (!ReservedHotkeys.Contains(userChoice.ToLower()))
            {
                Draw();
                userChoice = Console.ReadLine()?.Trim().ToLower();
                if (userChoice != null && MenuItems.ContainsKey(userChoice))
                {

                    if (MenuItems[userChoice].MethodToRun != null)
                    {
                        var result = MenuItems[userChoice].MethodToRun!();
                        if (result!.ToLower() == "x")
                        {
                            userChoice = result;
                        }

                        if (result.ToLower() == "b")
                        {
                            Console.Clear();
                        }

                        if (result.ToLower() == "r" && MenuLevel != EMenuLevel.Main)
                        {
                            Console.Clear();
                            userChoice = result;
                        }
                    }
                }
                else if (!ReservedHotkeys.Contains(userChoice?.ToLower()))
                {
                    Console.WriteLine("Unknown input!");
                }
            }
            /*if (userChoice.ToLower() == "x")
            {
                return userChoice;
            }
            if (userChoice.ToLower() == "r" && MenuLevel == EMenuLevel.Main)
            {
                Console.Clear();
                Console.WriteLine("Already in main menu");
                userChoice = "";
                continue;
            }
            if (userChoice.ToLower() == "b")
            {
                Console.Clear();
                Console.WriteLine("Already in main menu");
                userChoice = "";
            }
            }*/
            return userChoice;
    }
}

