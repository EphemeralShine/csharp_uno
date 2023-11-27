using ConsoleApp;
using DAL;
using Domain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using UIConsole;
using UnoEngine;

// ================== SAVES =====================
// IGameRepository gameRepository = new GameRepositoryFileSystem();
var connectionString = "DataSource=<%temppath%>uno.db;Cache=Shared";
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
 var rules = new Rules();
// ================== GAME =====================
var mainMenu = ProgramMenus.GetMainMenu(
    NewGame,
    () => ProgramMenus.GetLoadGameMenu(gameRepository, LoadGame),
    ProgramMenus.GetOptionsMenu(rules),
    rules
);


mainMenu.Run();
return;

string? NewGame()
{
    var gameEngine = new GameEngine
    {
        State =
        {
            GameRules = rules
        }
    };
    PlayerSetup.ConfigurePlayers(gameEngine);
    gameEngine.InitializeGameStartingState();
    var gameController = new GameController(gameEngine, gameRepository);
    gameController.GameLoop();
    return null;
}

string? LoadGame(Guid gameId)
{
    var gameEngine = new GameEngine();
    Console.WriteLine($"Loading file: {gameId}");
    var gameState = gameRepository.LoadGame(gameId);
    gameEngine.State = gameState;
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

