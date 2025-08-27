using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using MinhaAcademiaTEM.Application.Caching;

namespace MinhaAcademiaTEM.UnitTests.Application.Caching;

public class CacheOptionsProviderTests
{
    private static TimeSpan? TTL(string key)
    {
        MemoryCacheEntryOptions opt = CacheOptionsProvider.ForKey(key);
        return opt.AbsoluteExpirationRelativeToNow;
    }

    [Fact]
    public void ForKey_Should_Return_ThirtyDays_For_Equipments_And_User_Scoped()
    {
        TTL("coach_123_equipments").Should().Be(TimeSpan.FromDays(30)); // contains "equipments"
        TTL("user_abc_available_equipments_selections").Should().Be(TimeSpan.FromDays(30)); // starts with "user_"
    }

    [Fact]
    public void ForKey_Should_Return_TwelveHours_For_Totals()
    {
        TTL("users_page_1_pageSize_10_totalUsers_100").Should().Be(TimeSpan.FromHours(12)); // contains "total"
        TTL("coaches_page_1_pageSize_10_totalCoaches_50").Should().Be(TimeSpan.FromHours(12));
    }

    [Fact]
    public void ForKey_Should_Return_OneHour_For_Coach_Clients_And_Default()
    {
        TTL("coach_123_clients").Should().Be(TimeSpan.FromHours(1)); // starts "coach_" && contains "_clients"
        TTL("anything_else").Should().Be(TimeSpan.FromHours(1)); // default fallback
    }
}