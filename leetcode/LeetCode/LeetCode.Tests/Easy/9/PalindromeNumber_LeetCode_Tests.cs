using LeetCode.Solutions.Easy;

namespace LeetCode.Tests.Easy.PalindromeNumber;

public class LeetCode_Tests
{
    [Theory]
    [InlineData(121, true)]
    [InlineData(1234554321, true)]
    [InlineData(-121, false)]
    [InlineData(10, false)]
    [InlineData(9, true)]
    [InlineData(0, true)]
    [InlineData(12345, false)]
    public void LeetCode__Cases(int input, bool expected)
    {
        // Arrange
        var solution = new PalindromeNumber_Solution();

        // Act
        var result = solution.IsPalindrome(input);

        // Assert
        Assert.Equal(expected, result);
    }

}