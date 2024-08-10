namespace LeetCode.Solutions.Easy;

public class TwoRun_Solution
{
    public int[] TwoSum(int[] nums, int target)
    {
        Dictionary<int, int> result = new();

        for (int i = 0; i < nums.Length; i++)
        {
            int falta = target - nums[i];

            if (result.ContainsKey(falta))
                return [result[falta], i];

            if (!result.ContainsKey(nums[i]))
                result.Add(nums[i], i);
        }

        throw new ArgumentException("There is no solution");
    }
}