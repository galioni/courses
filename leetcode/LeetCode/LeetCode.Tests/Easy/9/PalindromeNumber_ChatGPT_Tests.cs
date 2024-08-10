using LeetCode.Solutions.Easy;

namespace LeetCode.Tests.Easy.PalindromeNumber;

public class ChatGpt_Tests
{
    [Theory]
    [InlineData(-121, false)]  // Negative number
    [InlineData(7, true)]      // Single digit number
    [InlineData(121, true)]    // Palindrome number
    [InlineData(123, false)]   // Non-palindrome number
    [InlineData(1221, true)]   // Palindrome with even digits
    [InlineData(12321, true)]  // Palindrome with odd digits
    [InlineData(1234, false)]  // Non-palindrome with even digits
    [InlineData(0, true)]      // Zero
    public void IsPalindrome_ReturnsExpectedResult(int input, bool expected)
    {
        // Arrange
        var palindromeChecker = new PalindromeNumber_Solution();

        // Act
        bool result = palindromeChecker.IsPalindrome(input);

        // Assert
        Assert.Equal(expected, result);
    }
}