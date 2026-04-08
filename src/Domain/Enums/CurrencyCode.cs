using System.ComponentModel;

namespace Project.Domain.Enums;

public enum CurrencyCode
{
    [Description("USD")]
    USD = 1,
    [Description("EUR")]
    EUR = 2,
    [Description("GBP")]
    GBP = 3,
    [Description("PKR")]
    PKR = 4,
}
