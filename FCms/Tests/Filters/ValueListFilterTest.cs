using Xunit;
using System.Collections.Generic;
using System;
using FCms.Content;

namespace FCms.Content.Tests;

[Trait("Category", FCmsTests.DbTests.DbHelpersTest.TEST_CATEGORY_BASIC)]
public class ValueListFilterTest
{
    private readonly ValueListFilter filter;

    public ValueListFilterTest()
    {
        filter = new ValueListFilter
        {
            Id = Guid.NewGuid(),
            Name = "TestFilter",
            DisplayName = "Test Filter"
        };
    }

    [Fact]
    public void Type_ShouldReturnValueList()
    {
        Assert.Equal("ValueList", filter.Type);
    }

    [Fact]
    public void Values_SetAndGet_ShouldWork()
    {
        // Arrange
        var testValues = new List<string> { "value1", "value2" };
        
        // Act
        filter.Values = testValues;
        
        // Assert
        Assert.Equal(testValues, filter.Values);
    }

    [Fact]
    public void Validate_WithNullValues_ShouldReturnFalse()
    {
        Assert.False(filter.Validate(null, "anyValue"));
    }

    [Theory]
    [InlineData("value1")]
    [InlineData("value2")]
    public void Validate_WithExistingValue_ShouldReturnTrue(string valueToCheck)
    {
        // Arrange
        var values = new List<object> { "value1", "value2" };
        
        // Act & Assert
        Assert.True(filter.Validate(values, valueToCheck));
    }

    [Theory]
    [InlineData("value3")]
    [InlineData("nonexistent")]
    public void Validate_WithNonExistingValue_ShouldReturnFalse(string valueToCheck)
    {
        // Arrange
        var values = new List<object> { "value1", "value2" };
        
        // Act & Assert
        Assert.False(filter.Validate(values, valueToCheck));
    }

    [Fact]
    public void ParseValues_ShouldConvertStringsToObjects()
    {
        // Arrange
        var stringValues = new List<string> { "value1", "value2" };
        
        // Act
        var result = filter.ParseValues(stringValues);

        // Assert
        Assert.Equal(stringValues.Count, result.Count);
        Assert.IsType<string>(result[0]);
        Assert.Equal(stringValues, result.ConvertAll(x => x.ToString()));
    }
}
