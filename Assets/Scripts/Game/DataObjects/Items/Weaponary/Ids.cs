using System.ComponentModel;

public enum WeaponID
{
    M16, M82, ACP, M1911, Vector, G36C, BarretM107, AK47
}

public enum ScopeID
{
    [Description("none")]
    none,
    [Description("HD")]
    x1HD,
    [Description("QX")]
    x1QX,
    [Description("x2")]
    x2,
    [Description("x3")]
    x3,
    [Description("x4")]
    x4,
    [Description("x6")]
    x6,
    [Description("x8")]
    x8,
    [Description("x8V2")]
    x8V2
}