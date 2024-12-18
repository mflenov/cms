using System;
using System.Collections.Generic;
using FCms.Content;
using Xunit;

namespace FCms.Tests.Filters;

[Trait("Category", FCmsTests.DbTests.DbHelpersTest.TEST_CATEGORY_BASIC)]
public class BooleanFilterTest
{
    private readonly BooleanFilter _filter;

    public BooleanFilterTest()
    {
        _filter = new BooleanFilter
        {
            Id = Guid.NewGuid(),
            Name = "TestFilter",
            DisplayName = "Test Boolean Filter"
        };
    }

    [Fact]
    public void Type_ShouldReturnBoolean()
    {
        Assert.Equal("Boolean", _filter.Type);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Validate_WhenValuesIsNull_ShouldReturnFalseForTrueAndTrueForFalse(bool inputValue)
    {
        var result = _filter.Validate(null, inputValue);
        Assert.Equal(!inputValue, result);
    }

    [Fact]
    public void Validate_WhenValuesContainsValue_ShouldReturnTrue()
    {
        var values = new List<object> { true };
        var result = _filter.Validate(values, true);
        Assert.True(result);
    }

    [Fact]
    public void Validate_WhenValuesDoesNotContainValue_ShouldReturnFalse()
    {
        var values = new List<object> { true };
        var result = _filter.Validate(values, false);
        Assert.False(result);
    }

    [Fact]
    public void ParseValues_WhenListIsEmpty_ShouldReturnListWithFalse()
    {
        var result = _filter.ParseValues(new List<string>());
        Assert.Single(result);
        Assert.False((bool)result[0]);
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("True", true)]
    [InlineData("false", false)]
    [InlineData("False", false)]
    public void ParseValues_ShouldCorrectlyParseValues(string input, bool expected)
    {
        var result = _filter.ParseValues(new List<string> { input });
        Assert.Single(result);
        Assert.Equal(expected, result[0]);
    }

    [Fact]
    public void ParseValues_WhenListIsNull_ShouldReturnEmptyList()
    {
        var result = _filter.ParseValues(null);
        Assert.Empty(result);
    }
}
