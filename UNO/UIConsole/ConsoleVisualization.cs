using Domain;
using Helpers;

namespace UIConsole;

public class ConsoleVisualization
{
    public static void DrawDesk(GameState state)
    {
        Console.WriteLine(state.CardToBeat!.CardColor == ECardColor.Black
            ? $"Current card to beat: {state.CardToBeat} and the color chosen is {state.CurrentColor!.Description()}"
            : $"Current card to beat: {state.CardToBeat}");
    }

    public static void DrawPlayerHand(Player player)
    {
        Console.WriteLine("Your current hand is:\n" +
                          string.Join(
                              "\n",
                              player.PlayerHand.Select((c, i) => (i+1) + ": " + c)
                          )
        );
    }

}