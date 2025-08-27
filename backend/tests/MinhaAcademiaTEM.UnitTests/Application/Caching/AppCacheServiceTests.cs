using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using MinhaAcademiaTEM.Application.Caching;

namespace MinhaAcademiaTEM.UnitTests.Application.Caching;

public class AppCacheServiceTests
{
    private static AppCacheService NewService()
    {
        var mem = new MemoryCache(new MemoryCacheOptions());
        return new AppCacheService(mem);
    }

    [Fact]
    public void Set_Then_TryGetValue_Should_Return_Stored_Value()
    {
        var cache = NewService();
        const string key = "coach_123_equipments";

        var value = new List<int> { 1, 2, 3 };
        cache.Set(key, value);

        var ok = cache.TryGetValue(key, out List<int>? got);

        ok.Should().BeTrue();
        got.Should().NotBeNull().And.BeEquivalentTo(value);
    }

    [Fact]
    public void TryGetValue_Should_Return_False_On_Type_Mismatch()
    {
        var cache = NewService();
        cache.Set("k", "string_value");

        var ok = cache.TryGetValue("k", out int? _);

        ok.Should().BeFalse();
    }

    [Fact]
    public void Remove_Should_Delete_Key()
    {
        var cache = NewService();
        cache.Set("k1", 42);

        cache.Remove("k1");

        cache.TryGetValue("k1", out int? _).Should().BeFalse();
    }

    [Fact]
    public void RemoveMultiple_Should_Delete_All_Given_Keys()
    {
        var cache = NewService();
        cache.Set("k1", 1);
        cache.Set("k2", 2);

        cache.RemoveMultiple("k1", "k2");

        cache.TryGetValue("k1", out int? _).Should().BeFalse();
        cache.TryGetValue("k2", out int? _).Should().BeFalse();
    }
}