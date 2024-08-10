import unittest
from sort_array import findSmallest, sort_ascending

class SortArray_Tests(unittest.TestCase):
    
    def test_find_smallest(self):
        self.assertTrue(findSmallest([156, 141, 35, 94, 88, 61, 111]), 35)
        self.assertTrue(findSmallest([156, 141, 94, 88, 61, 111]), 94)
        self.assertTrue(findSmallest([100, 100, 1, 100, 100]), 1)
    
    def test_sort_ascending(self):
        # Test with a general case
        self.assertEqual(sort_ascending([156, 141, 35, 94, 88, 61, 111]), [35, 61, 88, 94, 111, 141, 156])
        
        # Test with an already sorted array
        self.assertEqual(sort_ascending([3, 2, 1]), [1, 2, 3])
        
        # Test with an empty array
        self.assertEqual(sort_ascending([]), [])
        
        # Test with one element
        self.assertEqual(sort_ascending([1]), [1])
        
        # Test with duplicate elements
        self.assertEqual(sort_ascending([2, 3, 3, 1]), [1, 2, 3, 3])

if __name__ == '__main__':
    unittest.main()