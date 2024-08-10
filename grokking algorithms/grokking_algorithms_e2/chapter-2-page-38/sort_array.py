#35, 61, 88, 94, 111, 141, 156 - final result
import array

def findSmallest(arr):
    smallest = arr[0]

    for x in arr:
        if x < smallest:
            smallest = x

    return smallest

def sort_ascending(arr):
    if len(arr) == 0:
        return arr
    
    new_array = []
    copy_array = list(arr)
    
    for x in copy_array:
        smallest = findSmallest(arr)
        new_array.append(smallest)
        arr.remove(smallest)
    return new_array

print(sort_ascending([156, 141, 35, 94, 88, 61, 111]))
