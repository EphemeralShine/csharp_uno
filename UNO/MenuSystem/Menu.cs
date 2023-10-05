using System.Reflection.Emit;

namespace MenuSystem;

public class Menu
{
    public string? Title { get; set; }
    public Dictionary<string, MenuItem> MenuItems { get; set; } = new();
    private const string MenuSeparator = "======================";
    private static readonly HashSet<string> ReservedHotkeys = new() {"x", "b", "r"};

    public Menu(string? title, List<MenuItem> menuItems)
    {
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
        // TODO: should not be there in the main level
        Console.WriteLine("b) Back");
        // TODO: should not be there in the main and second level
        Console.WriteLine("r) Return to main");

        Console.WriteLine("x) eXit");

        Console.WriteLine(MenuSeparator);
        Console.Write("Your choice:");

    }

    public string Run()
    {
        Console.Clear();
        var userChoice = "";
        do
        {
            Draw();
            userChoice = Console.ReadLine()?.Trim().ToLower();
            if (userChoice != null && MenuItems.ContainsKey(userChoice))
            {
                if (MenuItems[userChoice].MethodToRun != null)
                {
                    var result = MenuItems[userChoice].MethodToRun!();
                    Console.WriteLine(result);
                }
            }
            else if (!ReservedHotkeys.Contains(userChoice))
            {
                Console.WriteLine("Unknown input!");
            }
        } while (!ReservedHotkeys.Contains(userChoice!));

        return userChoice;
    }
}

