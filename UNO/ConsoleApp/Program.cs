using MenuSystem;

var submenu2 = new Menu("Submenu2", new List<MenuItem>()
{
    new MenuItem()
    {
        Hotkey = "c",
        Label = "Player count: "
        //TODO: player count
    },
    new MenuItem()
    {
        Hotkey = "t",
        Label = "Player names and types: ",
        //TODO: player types
    } }, EMenuLevel.Sub2);

    submenu2.MenuItems.Add("s", new MenuItem()
    {
        Hotkey = "s",
        Label = "Start the game of UNO",
        MethodToRun = submenu2.Run
    });

var submenu1 = new Menu("Submenu1", new List<MenuItem>()
{
    new MenuItem()
    {
        Hotkey = "c",
        Label = "Player count: "
        //TODO: player count
    },
    new MenuItem()
    {
        Hotkey = "t",
        Label = "Player names and types: ",
        //TODO: player types
    },
    new MenuItem()
    {
        Hotkey = "s",
        Label = "Start the game of UNO",
        //TODO: Start game 
        MethodToRun = submenu2.Run
    }}, EMenuLevel.Sub1);

var mainMenu = new Menu("New Game", new List<MenuItem>()
{
    new MenuItem()
    {
        Hotkey = "c",
        /*Label = "Player count: " + game.Players.Count,*/
        //TODO: player count
    },
    new MenuItem()
    {
        Hotkey = "t",
        Label = "Player names and types: ",
        //TODO: player types
    },
    new MenuItem()
    {
        Hotkey = "s",
        Label = "Start the game of UNO",
        //TODO: Start game 
        MethodToRun = submenu1.Run
    },
});


var userChoice = mainMenu.Run();

