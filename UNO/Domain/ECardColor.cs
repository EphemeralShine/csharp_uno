using System.ComponentModel;

namespace Domain;

public enum ECardColor
{
    [Description("🔴")]
    Red,
    [Description("🔵")]
    Blue,
    [Description("🟢")]
    Green,
    [Description("🟡")]
    Yellow,
    [Description("⚫")]
    Black,
    None
}