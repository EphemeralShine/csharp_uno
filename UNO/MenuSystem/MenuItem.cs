namespace MenuSystem;

public class MenuItem
{
    public string Label { get; set; } = default!;
    public Func<string>? MenuLabelFunction { get; set; }
    public string Hotkey { get; set; } = default!;

    public Func<string?>? MethodToRun { get; set; } = null;
    
}