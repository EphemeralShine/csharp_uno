using ConsoleApp;
using DAL;
using Domain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using UIConsole;
using UnoEngine;

// ================== SAVES =====================
// IGameRepository gameRepository = new GameRepositoryFileSystem();
var connectionString = "DataSource=<%temppath%>durak.db;Cache=Shared";
connectionString = connectionString.Replace("<%temppath%>", Path.GetTempPath());


var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite(connectionString)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .Options;
using var db = new AppDbContext(contextOptions);
// apply all the migrations
db.Database.Migrate();
IGameRepository gameRepository = new GameRepositoryEF(db);


// ================== GAME =====================
var mainMenu = ProgramMenus.GetMainMenu(
    NewGame,
    LoadGame
);


mainMenu.Run();
return;

string? NewGame()
{
    var gameEngine = new GameEngine();
    PlayerSetup.ConfigurePlayers(gameEngine);
    gameEngine.InitializeGameStartingState();
    var gameController = new GameController(gameEngine, gameRepository);
    gameController.GameLoop();
    return null;
}

string? LoadGame()
{
    Console.WriteLine("Saved games");
    var saveGameList = gameRepository.GetSaveGames();
    var saveGameListDisplay = saveGameList.Select((s, i) => (i + 1) + " - " + s).ToList();

    if (saveGameListDisplay.Count == 0) return null;

    Guid gameId;
    while (true)
    {
        Console.WriteLine(string.Join("\n", saveGameListDisplay));
        var prompt = $"Select game to load (1..{saveGameListDisplay.Count}):";
        int userChoice = Prompts.Prompt<int>(prompt, "^([1-9]|[1-9][0-9]|99)$");
        if (userChoice < 1 || userChoice > saveGameListDisplay.Count)
        { 
            Console.WriteLine("Not in range...");
            continue;
        }
        gameId = saveGameList[userChoice - 1].id;
        Console.WriteLine($"Loading file: {gameId}");
        break;
    }

    var gameState = gameRepository.LoadGame(gameId);

    var gameEngine = new GameEngine()
    {
        State = gameState
    };
    
    var gameController = new GameController(gameEngine, gameRepository);

    gameController.GameLoop();

    return null;
}
















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

mainMenu = new Menu("<<< ||U||N||O|| >>>", new List<MenuItem>()
{
    new()
    {
        Hotkey = "s",
        Label = "Start the game of UNO",
        MethodToRun = submenu1.Run
    },
    new()
    {
        Hotkey = "p",
        /*Label = "Player count: " + game.Players.Count,*/
        //TODO: player count
    },
    new()
    {
        Hotkey = "r",
        Label = "Alter rules",
        //TODO: player types
    },
});


var userChoice = mainMenu.Run();

