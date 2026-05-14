# About
This repository contains custom implementation of priority queue using binary heap and Dictionary for searching and updating values.</br>
So overall time complexity is O(log(n)). It could be slightly improved, but with the price of removing Update and TryFind methods,</br>
beacuse maintaining Dictionary synchronized with binary heap costs extra time, but it is constant.

# Usage
Parameterless constructor `CustomPriorityQueue<TValue, TPriority>()` for values implementing `IComparable`.</br>
Paramtric constructor `CustomPriorityQueue<TValue, TPriority>(Comparison<TPriority> comparison)` for values not implementing `IComparable`.</br>
  * `comparison` delegate for comparing values.

## Public methods
`Enqueue(TValue value, TPriority priority)` inserts `value` and `priority` to priority queue.</br>
`Top()` returns root (minimum for min-heap).</br>
`IsEmpty()` returns bool.</br>
