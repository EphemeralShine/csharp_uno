using ConsoleApp;
using DAL;
using Domain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using UIConsole;
using UnoEngine;

// ================== SAVES =====================
IGameRepository gameRepository = new GameRepositoryFileSystem();
/*var connectionString = "DataSource=<%temppath%>uno.db;Cache=Shared";
connectionString = connectionString.Replace("<%temppath%>", Path.GetTempPath());


var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite(connectionString)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .Options;
using var db = new AppDbContext(contextOptions);
// apply all the migrations
db.Database.Migrate();
IGameRepository gameRepository = new GameRepositoryEF(db);*/
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
    var res = gameController.GameLoop();
    return res == "r" ? "r" : null;
}



