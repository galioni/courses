namespace LeetCode.Solutions.Easy;

public class PalindromeNumber_Solution
{
    public bool IsPalindrome(int x)
    {
        if (x < 0)
            return false;

        if (x >= 0 && x < 10)
            return true;

        var numbers = x.ToString().Reverse().ToArray();

        string result = string.Join("", numbers.Select(n => n.ToString()));

        return x.ToString() == result;

        
    }
    
    //LeetCode best solution
    public bool LeetCode_IsPalindrome(int x)
    {
        if (x < 0) return false;
        int y = 0;
        int tmp = x;

        while (tmp != 0)
        {
            int p = tmp % 10;
            y = y * 10 + p;
            tmp /= 10;
        }
        return (x == y);
    }
}
