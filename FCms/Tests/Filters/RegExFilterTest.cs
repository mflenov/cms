using System;
using System.Collections.Generic;
using Xunit;
using FCms.Content;

namespace FCms.Tests.Filters;

[Trait("Category", FCmsTests.DbTests.DbHelpersTest.TEST_CATEGORY_BASIC)]
public class RegExFilterTests
{
    private readonly RegExFilter _filter;

    public RegExFilterTests()
    {
        _filter = new RegExFilter
        {
            Id = Guid.NewGuid(),
            Name = "TestRegEx",
            DisplayName = "Test RegEx Filter"
        };
    }

    [Fact]
    public void Type_ShouldReturnRegEx()
    {
        Assert.Equal("RegEx", _filter.Type);
    }

    [Fact]
    public void Validate_WhenValuesIsNull_ReturnsFalse()
    {
        Assert.False(_filter.Validate(null, "test"));
    }

    [Theory]
    [InlineData("test", "^test$", true)]
    [InlineData("test123", "\\d+$", true)]
    [InlineData("abc", "^test", false)]
    [InlineData("TEST", "^test$", true)] // проверка игнорирования регистра
    public void Validate_WithPattern_ReturnsExpectedResult(string input, string pattern, bool expected)
    {
        var values = new List<object> { pattern };
        var result = _filter.Validate(values, input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Validate_WithMultiplePatterns_ReturnsTrue_WhenAnyMatches()
    {
        var patterns = new List<object> { "^test$", "\\d+$", "^abc" };
        Assert.True(_filter.Validate(patterns, "abc123"));
    }

    [Fact]
    public void ParseValues_ConvertsStringsToObjects()
    {
        var input = new List<string> { "pattern1", "pattern2" };
        var result = _filter.ParseValues(input);

        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.IsType<string>(item));
        Assert.Equal(input[0], result[0]);
        Assert.Equal(input[1], result[1]);
    }
}
