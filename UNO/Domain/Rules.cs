namespace Domain;

public class Rules
{
    public int HandSize { get; set; } = 7;
    public bool MultipleCardMoves { get; set; } = true;

    public override string ToString() => $"hand size: {HandSize}, allow multiple card moves: {(MultipleCardMoves ? "yes": "no")}";
}
