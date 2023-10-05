namespace MenuSystem;

public class MenuItem
{
    public string Label { get; set; } = default!;
    public string Hotkey { get; set; } = default!;

    public Func<string>? MethodToRun { get; set; } = null;
}