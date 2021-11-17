import Foundation

func taskTimeConsuming(task: () -> Void) -> TimeInterval {
    let startTime = Date()
    task()
    let endTime = Date()
    return endTime.timeIntervalSince(startTime)
}

extension Array {
    public mutating func exchange(element1: Array.Index, element2: Array.Index) {
        let temp = self[element1]
        self[element1] = self[element2]
        self[element2] = temp
    }
}

public class Quick {
    public static func sort<Element: Comparable>(array: inout [Element]) {
        sort(array: &array, lo: 0, hi: array.count - 1)
    }
    public static func sort<Element: Comparable>(array: inout [Element], lo: Array.Index, hi: Array.Index) {
        if hi <= lo {
            return
        }
        let j = partition(array: &array, lo: lo, hi: hi)
        sort(array: &array, lo: lo, hi: j - 1)
        sort(array: &array, lo: j + 1, hi: hi)
    }
    private static func partition<Element: Comparable>(array: inout [Element], lo: Array.Index, hi: Array.Index) -> Array.Index {
        var i = lo // 左扫描指针
        var j = hi + 1  // 右扫描指针
        let v = array[lo]   // 切分元素
        while true {
            // 扫描左右，检查扫描是否结束并交换元素
            i += 1
            while array[i] < v {
                if i == hi {
                    break
                }
                i += 1
            }
            j -= 1
            while v < array[j] {
                if j == lo {
                    break
                }
                j -= 1
            }
            if i >= j {
                break
            }
            array.exchange(element1: i, element2: j)
        }
        array.exchange(element1: lo, element2: j)
        return j
    }
}

func generateIntArray(size: Int) -> [Int] {
    var copy = [Int](repeating: 0, count: size)
    // 这里使用并发的方式填充数组加快速度
    DispatchQueue.concurrentPerform(iterations: size) { i in
        copy[i] = Int.random(in: 0..<Int.max)
    }
    return copy
}

var aa = generateIntArray(size: 1000000000)
var bb = aa
let aaSortTime = taskTimeConsuming {
    aa.sort()
}
print("标准库排序耗时：\(aaSortTime)")

let bbSortTime = taskTimeConsuming {
    Quick.sort(array: &bb)
}
print("QuickSort耗时：\(bbSortTime)")

print("aa == bb –> \(aa == bb)")

