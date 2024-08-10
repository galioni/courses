using LeetCode.Solutions.Easy;

namespace LeetCode.Tests.Easy.TwoSum;

public class LeetCode_Tests
{
    [Fact]
    public void LeetCode__Case_1()
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
    public void LeetCode__Case_2()
    {
        // Arrange
        var solution = new TwoRun_Solution();
        int[] nums = { 3, 2, 4 };
        int target = 6;
        int[] expected = [1, 2];

        // Act
        int[] result = solution.TwoSum(nums, target);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void LeetCode__Case_3()
    {
        // Arrange
        var solution = new TwoRun_Solution();
        int[] nums = { 3, 3 };
        int target = 6;
        int[] expected = { 0, 1 };

        // Act
        int[] result = solution.TwoSum(nums, target);

        // Assert
        Assert.Equal(expected, result);
    }
}