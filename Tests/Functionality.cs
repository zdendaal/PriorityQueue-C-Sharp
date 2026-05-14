using BinaryHeap;

namespace Tests
{
    public class Functionality
    {
        [Fact]
        public void Dequeue_ValuesImplementingIComparableRandomOrder_ShouldReturnElementsInPriorityOrder()
        {
            // arange
            BinaryHeap<int, int> heap = new BinaryHeap<int, int>();
            // act
            heap.Enqueue(25, 9);
            heap.Enqueue(10, 11);
            heap.Enqueue(8, 4);
            heap.Enqueue(34, 7);
            heap.Enqueue(26, 52);
            heap.Enqueue(-6, -15);
            heap.Enqueue(2, 2);
            heap.Enqueue(7, 3);
            // assert
            Assert.Equal(-6, heap.Dequeue().Value);
            Assert.Equal(2, heap.Dequeue().Value);
            Assert.Equal(7, heap.Dequeue().Value);
            Assert.Equal(8, heap.Dequeue().Value);
            Assert.Equal(34, heap.Dequeue().Value);
            Assert.Equal(25, heap.Dequeue().Value);
            Assert.Equal(10, heap.Dequeue().Value);
            Assert.Equal(26, heap.Dequeue().Value);
        }

        [Fact]
        public void Dequeue_ValuesImplementingIComparableAscendingOrder_ShouldReturnElementsInPriorityOrder()
        {
            BinaryHeap<int, int> heap = new BinaryHeap<int, int>();

            heap.Enqueue(25, 0);
            heap.Enqueue(10, 1);
            heap.Enqueue(8, 2);
            heap.Enqueue(34, 3);
            heap.Enqueue(26, 4);
            heap.Enqueue(-6, 5);
            heap.Enqueue(2, 6);
            heap.Enqueue(7, 7);

            Assert.Equal(25, heap.Dequeue().Value);
            Assert.Equal(10, heap.Dequeue().Value);
            Assert.Equal(8, heap.Dequeue().Value);
            Assert.Equal(34, heap.Dequeue().Value);
            Assert.Equal(26, heap.Dequeue().Value);
            Assert.Equal(-6, heap.Dequeue().Value);
            Assert.Equal(2, heap.Dequeue().Value);
            Assert.Equal(7, heap.Dequeue().Value);
        }

        [Fact]
        public void Dequeue_ValuesImplementingIComparableDescendingOrder_ShouldReturnElementsInPriorityOrder()
        {
            BinaryHeap<int, int> heap = new BinaryHeap<int, int>();

            heap.Enqueue(25, 7);
            heap.Enqueue(10, 6);
            heap.Enqueue(8, 5);
            heap.Enqueue(34, 4);
            heap.Enqueue(26, 3);
            heap.Enqueue(-6, 2);
            heap.Enqueue(2, 1);
            heap.Enqueue(7, 0);

            Assert.Equal(7, heap.Dequeue().Value);
            Assert.Equal(2, heap.Dequeue().Value);
            Assert.Equal(-6, heap.Dequeue().Value);
            Assert.Equal(26, heap.Dequeue().Value);
            Assert.Equal(34, heap.Dequeue().Value);
            Assert.Equal(8, heap.Dequeue().Value);
            Assert.Equal(10, heap.Dequeue().Value);
            Assert.Equal(25, heap.Dequeue().Value);
        }

        [Fact]
        public void Dequeue_ValuesNotImplementingIComparableRandomOrder_ShouldReturnElementsInPriorityOrder()
        {
            // defining custom comparison for more complext data structures
            Comparison<Value> comparison = delegate (Value a, Value b) {
                if (a.id < b.id) return -1;
                else if (a.id > b.id) return 1;
                int nameResult = a.name.CompareTo(b.name);
                if (nameResult < 0) return -1;
                else if (nameResult > 0) return 1;
                else if (a.value < b.value) return -1;
                else if (a.value > b.value) return 1;
                else return 0;
            };

            BinaryHeap<Value, Value> heap = new BinaryHeap<Value, Value>(comparison);
            Value[] values = new Value[]
            {
                new Value(1, "abcd", 2),
                new Value(0, "kqla", 1),
                new Value(2, "laeud", 3),
                new Value(0, "aaaa", 8),
                new Value(0, "abbb", 6),
                new Value(0, "abcc", 4),
                new Value(2, "beee", 5),
                new Value(1, "aert", 7)
            };

            heap.Enqueue(values[0], values[0]);
            heap.Enqueue(values[1], values[1]);
            heap.Enqueue(values[2], values[2]);
            heap.Enqueue(values[3], values[3]);
            heap.Enqueue(values[4], values[4]);
            heap.Enqueue(values[5], values[5]);
            heap.Enqueue(values[6], values[6]);
            heap.Enqueue(values[7], values[7]);

            Assert.Equal(values[3], heap.Dequeue().Value);
            Assert.Equal(values[4], heap.Dequeue().Value);
            Assert.Equal(values[5], heap.Dequeue().Value);
            Assert.Equal(values[1], heap.Dequeue().Value);
            Assert.Equal(values[0], heap.Dequeue().Value);
            Assert.Equal(values[7], heap.Dequeue().Value);
            Assert.Equal(values[6], heap.Dequeue().Value);
            Assert.Equal(values[2], heap.Dequeue().Value);
        }

        [Fact]
        public void Dequeue_ValuesNotImplementingIComparableDuplicitPriority_ShouldReturnElementsInPriorityOrder()
        {
            // defining custom comparison for more complext data structures
            Comparison<Value> comparison = delegate (Value a, Value b) {
                if (a.id < b.id) return -1;
                else if (a.id > b.id) return 1;
                int nameResult = a.name.CompareTo(b.name);
                if (nameResult < 0) return -1;
                else if (nameResult > 0) return 1;
                else if (a.value < b.value) return -1;
                else if (a.value > b.value) return 1;
                else return 0;
            };

            BinaryHeap<Value, Value> heap = new BinaryHeap<Value, Value>(comparison);
            Value[] values = new Value[]
            {
                new Value(1, "abcd", 2),
                new Value(0, "kqla", 1),
                new Value(2, "laeud", 3),
                new Value(0, "aaaa", 8),
                new Value(0, "abbb", 6),
                new Value(0, "abcc", 4),
                new Value(2, "beee", 5),
                new Value(1, "aert", 7)
            };

            heap.Enqueue(values[0], values[0]);
            heap.Enqueue(values[1], values[3]);
            heap.Enqueue(values[2], values[3]);
            heap.Enqueue(values[3], values[3]);
            heap.Enqueue(values[4], values[0]);
            heap.Enqueue(values[5], values[0]);
            heap.Enqueue(values[6], values[6]);
            heap.Enqueue(values[7], values[6]);

            Assert.Equal(values[1], heap.Dequeue().Value);
            Assert.Equal(values[3], heap.Dequeue().Value);
            Assert.Equal(values[2], heap.Dequeue().Value);
            Assert.Equal(values[0], heap.Dequeue().Value);
            Assert.Equal(values[4], heap.Dequeue().Value);
            Assert.Equal(values[5], heap.Dequeue().Value);
            Assert.Equal(values[7], heap.Dequeue().Value);
            Assert.Equal(values[6], heap.Dequeue().Value);
        }

        [Fact]
        public void TryFind_ValuesImplementingIComparable_ShouldFindAll()
        {
            BinaryHeap<int, int> heap = new BinaryHeap<int, int>();

            heap.Enqueue(25, 0);
            heap.Enqueue(10, 1);
            heap.Enqueue(8, 2);
            heap.Enqueue(3, 3);
            heap.Enqueue(3, 4);
            heap.Enqueue(-6, 5);
            heap.Enqueue(2, 6);
            heap.Enqueue(3, 7);

            Assert.True(heap.TryFind(3, out var values));
            var priorities = values.Select(x => x.Priority);
            Assert.Contains(3, priorities);
            Assert.Contains(4, priorities);
            Assert.Contains(7, priorities);
            Assert.Equal(3, priorities.Count());
        }

        [Fact]
        public void Update_Value_ShouldUpdate()
        {
            BinaryHeap<int, int> heap = new BinaryHeap<int, int>();

            heap.Enqueue(25, 7);
            heap.Enqueue(10, 6);
            heap.Enqueue(8, 5);
            heap.Enqueue(34, 4);
            heap.Enqueue(26, 3);
            heap.Enqueue(-6, 2);
            heap.Enqueue(2, 1);
            heap.Enqueue(7, 0);

            heap.TryFind(34, out var values1);
            heap.TryFind(2, out var values2);
            heap.Update(values1[0].Index, 100, 15);
            heap.Update(values2[0].Index, -5, -3);

            Assert.Equal(-5, heap.Dequeue().Value);
            Assert.Equal(7, heap.Dequeue().Value);
            Assert.Equal(-6, heap.Dequeue().Value);
            Assert.Equal(26, heap.Dequeue().Value);
            Assert.Equal(8, heap.Dequeue().Value);
            Assert.Equal(10, heap.Dequeue().Value);
            Assert.Equal(25, heap.Dequeue().Value);
            Assert.Equal(100, heap.Dequeue().Value);
        }
    }
}
