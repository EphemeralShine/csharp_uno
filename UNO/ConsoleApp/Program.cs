// See https://aka.ms/new-console-template for more information

using System.ComponentModel.Design;
using MenuSystem;

var NewGameMenu = new Menu("New Game", new List<MenuItem>()
{
    new MenuItem()
    {
        Hotkey = "c",
        Label = "Player count: " + game.Players.Count,
    },
    new MenuItem()
    {
        Hotkey = "t",
        Label = "Player names and types: ",
        //TODO: player count
    },
    new MenuItem()
    {
        Hotkey = "s",
        Label = "Start the game of UNO",
        //TODO: Start game 
    },
});

