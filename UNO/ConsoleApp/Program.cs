using MenuSystem;

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
    }});

var newGameMenu = new Menu("New Game", new List<MenuItem>()
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


var userChoice = newGameMenu.Run();

