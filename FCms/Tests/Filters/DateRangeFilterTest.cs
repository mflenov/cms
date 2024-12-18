using System;
using System.Collections.Generic;
using Xunit;
using FCms.Content;

namespace FCms.Tests.Filters;

[Trait("Category", FCmsTests.DbTests.DbHelpersTest.TEST_CATEGORY_BASIC)]
public class DateRangeFilterTest
{
    private readonly DateRangeFilter _filter;

    public DateRangeFilterTest()
    {
        _filter = new DateRangeFilter
        {
            Id = Guid.NewGuid(),
            Name = "TestDateRange",
            DisplayName = "Test Date Range"
        };
    }

    [Fact]
    public void Type_ShouldReturnDateRange()
    {
        Assert.Equal("DateRange", _filter.Type);
    }

    [Fact]
    public void Validate_WithinRange_ReturnsTrue()
    {
        // Arrange
        var values = new List<object>
        {
            new DateTime(2024, 1, 1),
            new DateTime(2024, 12, 31)
        };
        var testDate = new DateTime(2024, 6, 15);

        // Act
        var result = _filter.Validate(values, testDate);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Validate_OutsideRange_ReturnsFalse()
    {
        // Arrange
        var values = new List<object>
        {
            new DateTime(2024, 1, 1),
            new DateTime(2024, 12, 31)
        };
        var testDate = new DateTime(2025, 1, 1);

        // Act
        var result = _filter.Validate(values, testDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Validate_EmptyValues_UsesMinMaxDates()
    {
        // Arrange
        var values = new List<object>();
        var testDate = DateTime.Now;

        // Act
        var result = _filter.Validate(values, testDate);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ParseValues_ValidDates_ReturnsDateTimeList()
    {
        // Arrange
        var stringDates = new List<string> { "2024-01-01", "2024-12-31" };

        // Act
        var result = _filter.ParseValues(stringDates);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.IsType<DateTime>(result[0]);
        Assert.IsType<DateTime>(result[1]);
        Assert.Equal(new DateTime(2024, 1, 1), result[0]);
        Assert.Equal(new DateTime(2024, 12, 31), result[1]);
    }

    [Fact]
    public void ParseValues_NullInput_ReturnsEmptyList()
    {
        // Act
        var result = _filter.ParseValues(null);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ParseValues_InvalidDates_SkipsInvalidValues()
    {
        // Arrange
        var stringDates = new List<string> { "2024-01-01", "invalid-date", "2024-12-31" };

        // Act
        var result = _filter.ParseValues(stringDates);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(new DateTime(2024, 1, 1), result[0]);
        Assert.Equal(new DateTime(2024, 12, 31), result[1]);
    }
}
