using LeetCode.Solutions.Easy;

namespace LeetCode.Tests.Easy.TwoSum;

public class ChatGPT_Tests
{
    [Fact]
    public void ChatGTP__Returns_Correct_Indices()
    {
        // Arrange
        var solution = new TwoRun_Solution();
        int[] nums = { 2, 7, 11, 15 };
        int target = 9;
        int[] expected = { 0, 1 };

        // Act
        int[] result = solution.TwoSum(nums, target);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ChatGTP__Returns_First_Correct_Pair()
    {
        // Arrange
        var solution = new TwoRun_Solution();
        int[] nums = { 3, 2, 4 };
        int target = 6;
        int[] expected = { 1, 2 };

        // Act
        int[] result = solution.TwoSum(nums, target);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ChatGTP__Returns_Indices_For_Negative_Numbers()
    {
        // Arrange
        var solution = new TwoRun_Solution();
        int[] nums = { -1, -2, -3, -4, -5 };
        int target = -8;
        int[] expected = { 2, 4 };

        // Act
        int[] result = solution.TwoSum(nums, target);

        // Assert
        Assert.Equal(expected, result);
    }
}