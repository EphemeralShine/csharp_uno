using Domain;

namespace UIConsole;

public class ConsoleVisualization
{
    public static void DrawDesk(GameState state)
    {
        if (state.CardToBeat!.CardColor == ECardColor.Black)
        {
            Console.WriteLine($"Current card to beat: {state.CardToBeat} and the color chosen is {state.CurrentColor}");

        }

        Console.WriteLine($"Current card to beat: {state.CardToBeat}");
    }

    public static void DrawPlayerHand(Player player)
    {
        Console.WriteLine("Your current hand is: " +
                          string.Join(
                              "  ",
                              player.PlayerHand.Select((c, i) => (i+1) + ": " + c)
                          )
        );
    }

}