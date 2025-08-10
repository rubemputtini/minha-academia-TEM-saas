using Microsoft.Extensions.Caching.Memory;

namespace MinhaAcademiaTEM.Application.Caching;

public static class CacheOptionsProvider
{
    private static MemoryCacheEntryOptions OneHour => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
    };

    private static MemoryCacheEntryOptions TwelveHours => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
    };

    private static MemoryCacheEntryOptions OneDay => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
    };

    private static MemoryCacheEntryOptions ThreeDays => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3)
    };

    private static MemoryCacheEntryOptions SevenDays => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
    };

    private static MemoryCacheEntryOptions ThirtyDays => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
    };

    public static MemoryCacheEntryOptions ForKey(string key) =>
        key switch
        {
            var k when k.Contains("equipments") => ThirtyDays,
            var k when k.Contains("total") => TwelveHours,
            var k when k.StartsWith("coach_") && k.Contains("_clients") => OneHour,
            _ => OneHour
        };
}