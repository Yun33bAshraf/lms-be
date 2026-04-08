using System.ComponentModel;

namespace Project.Domain.Enums;

public enum Theme
{
    [Description("Light")]
    Light = 1,
    [Description("Dark")]
    Dark = 2,
    [Description("System")]
    System = 3
}
