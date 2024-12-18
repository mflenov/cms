using System;
using System.Collections.Generic;
using Xunit;
using FCms.Content;

namespace FCms.Tests.Filters;

[Trait("Category", FCmsTests.DbTests.DbHelpersTest.TEST_CATEGORY_BASIC)]
public class TextFilterTest
{
    private readonly TextFilter _filter;

    public TextFilterTest()
    {
        _filter = new TextFilter
        {
            Id = Guid.NewGuid(),
            Name = "TestFilter",
            DisplayName = "Test Filter"
        };
    }

    [Fact]
    public void Type_ShouldReturnText()
    {
        // Act
        var result = _filter.Type;

        // Assert
        Assert.Equal("Text", result);
    }

    [Fact]
    public void Validate_WhenValuesIsNull_ShouldReturnFalse()
    {
        // Act
        var result = _filter.Validate(null, "test");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Validate_WhenValueExists_ShouldReturnTrue()
    {
        // Arrange
        var values = new List<object> { "test1", "test2", "test3" };

        // Act
        var result = _filter.Validate(values, "test2");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Validate_WhenValueDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var values = new List<object> { "test1", "test2", "test3" };

        // Act
        var result = _filter.Validate(values, "test4");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ParseValues_ShouldConvertStringsToObjects()
    {
        // Arrange
        var stringList = new List<string> { "test1", "test2", "test3" };

        // Act
        var result = _filter.ParseValues(stringList);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.All(result, item => Assert.IsType<string>(item));
    }
}
