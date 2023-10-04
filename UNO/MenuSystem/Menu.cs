using System.Reflection.Emit;

namespace MenuSystem;

public class Menu
{
    public string? Title { get; set; }
    public List<MenuItem> MenuItems { get; set; } = new();
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

            foreach (var item in MenuItems)
            {
                if (item.Hotkey.Contains(menuItem.Hotkey.ToLower()))
                {
                    throw new ApplicationException(
                        $"Hotkey '{menuItem.Hotkey}' already in use!");
                }
            }
            MenuItems.Add(menuItem);
        }
    }
    
    private void Draw()
    {
        Console.WriteLine(Title);
        Console.WriteLine(MenuSeparator);
        foreach (var menuItem in MenuItems)
        {
            Console.Write($"{menuItem.Hotkey})");
            Console.WriteLine(menuItem.Label);
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
        Draw();
        userChoice = Console.ReadLine()?.Trim();
        if (String.IsNullOrWhiteSpace(userChoice))
        {
            throw new ApplicationException(
                "Provided empty input!");
        }
        foreach (var menuItem in MenuItems)
        {
            if (menuItem.Hotkey.ToLower().Contains(userChoice))
            {
                
            }
        }

        return userChoice;
    }
}

