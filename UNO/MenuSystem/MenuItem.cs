namespace MenuSystem;

public class MenuItem
{
    public string Label { get; set; } = default!;
    public string Hotkey { get; set; } = default!;

    public Action? MethodToRun { get; set; } = null;
}