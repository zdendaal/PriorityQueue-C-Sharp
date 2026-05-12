using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace BinaryHeap
{
    /// <summary>
    /// Implementation of priority queueu. Heap is min-heap by default.
    /// </summary>
    /// <typeparam name="TValue">Value.</typeparam>
    /// <typeparam name="TPriority">Priority according to are elements ordered.</typeparam>
    public class BinaryHeap<TValue, TPriority>
        where TValue : notnull
        where TPriority : notnull
    {

        private List<QueueElement<TValue, TPriority>> heap = new List<QueueElement<TValue, TPriority>>();

        private Dictionary<TValue, Dictionary<int, QueueElement<TValue, TPriority>>> fastAccess = new Dictionary<TValue, Dictionary<int, QueueElement<TValue, TPriority>>>();

        private readonly Comparison<TPriority> comparison;

        /// <summary>
        /// For case TPriority does implement IComparable interface.
        /// </summary>
        public BinaryHeap()
        {
            comparison = Comparer<TPriority>.Default.Compare;
        }

        /// <summary>
        /// For case TPriority does not implement IComparable interface
        /// </summary>
        /// <param name="comparison">Comparison for types that not implement IComparable interface</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BinaryHeap(Comparison<TPriority> comparison) 
        {
            this.comparison = comparison ?? throw new ArgumentNullException(nameof(comparison), "Method for comparison cannot be null.");
        }

        /// <summary>
        /// Adds an element with the specified value and priority to the priority queue. TValue has to be unique, heap will ignore adding duplicit TValue.
        /// </summary>
        /// <param name="value">The value to add to the queue.</param>
        /// <param name="priority">The priority associated with the value. Elements with lower priority values are dequeued before those with
        /// higher values.</param>
        public void Enqueue(TValue value, TPriority priority)
        {
            ref Dictionary<int, QueueElement<TValue, TPriority>> reference = ref CollectionsMarshal.GetValueRefOrNullRef(fastAccess, value);
            if (Unsafe.IsNullRef(ref reference))
                fastAccess[value] = new Dictionary<int, QueueElement<TValue, TPriority>>();

            heap.Add(new QueueElement<TValue, TPriority>(value, priority));
            int valueIndex = heap.Count() - 1;
            // bubble up
            int parentIndex = 1;
            QueueElement<TValue, TPriority> buffer;
            
            if (valueIndex % 2 == 0)
                parentIndex = (valueIndex - 2) / 2;
            else
                parentIndex = (valueIndex - 1) / 2;

            int oldParentIndex;
            while (parentIndex >= 0)
            {
                // switch parent and child
                if (comparison(heap[valueIndex].priority, heap[parentIndex].priority) < 0)
                {
                    buffer = heap[parentIndex];
                    heap[parentIndex] = heap[valueIndex];
                    heap[valueIndex] = buffer;
                    buffer.index = valueIndex;

                    oldParentIndex = parentIndex;
                    if (valueIndex % 2 == 0)
                    {
                        parentIndex = (valueIndex - 2) / 2;
                        valueIndex = oldParentIndex;
                    }
                    else
                    {
                        parentIndex = (valueIndex - 1) / 2;
                        valueIndex = oldParentIndex;
                    }
                }
                else
                    break;
            }

            heap[valueIndex].index = valueIndex;
            fastAccess[value][valueIndex] = heap[valueIndex]; 
        }

        /// <summary>
        /// Returns top element, which is elemenent accrding to comparison delegate. Throws InvalidOperationException if queue is empty
        /// </summary>
        /// <returns></returns>
        public IQueueElement<TValue, TPriority> Top()
        {
            if (!heap.Any())
                throw new InvalidOperationException("Queue is empty.");
            return heap[0];
        }

        /// <summary>
        /// Returns if queue is empty.
        /// </summary>
        /// <returns>true if empty, otherwise false</returns>
        public bool IsEmpty()
        {
            return heap.Any();
        }

        public IQueueElement<TValue, TPriority> Dequeue()
        {
            if (!heap.Any())
                throw new InvalidOperationException("Queue is empty.");
            var value = heap[0];
            heap[0] = heap[heap.Count() - 1];   // insert last element at 0 index

            // bubble down
            QueueElement<TValue, TPriority> buffer;
            int rightIndex = 2;
            int leftIndex = 1;
            int parentIndex = 0;
            while(rightIndex < heap.Count() - 1)
            {
                // Equels in second part => queue is stable
                if (comparison(heap[leftIndex].priority, heap[rightIndex].priority) < 0 && comparison(heap[parentIndex].priority, heap[leftIndex].priority) <= 0)
                {
                    buffer = heap[leftIndex];
                    heap[leftIndex] = heap[parentIndex];
                    heap[parentIndex] = buffer;

                    parentIndex = leftIndex;
                    rightIndex = 2 * parentIndex + 2;
                    leftIndex = rightIndex - 1;    
                }
                // Equels in second part => queue is stable
                if (comparison(heap[rightIndex].priority, heap[leftIndex].priority) < 0 && comparison(heap[parentIndex].priority, heap[rightIndex].priority) <= 0)
                {
                    buffer = heap[rightIndex];
                    heap[rightIndex] = heap[parentIndex];
                    heap[parentIndex] = buffer;

                    parentIndex = rightIndex;
                    rightIndex = 2 * parentIndex + 2;
                    leftIndex = rightIndex - 1;
                }
            }

            fastAccess.Remove(heap[0].value);
            return value;
        }

        public bool TryFind(TValue value, out List<IQueueElement<TValue, TPriority>> priority)
        {
            if (fastAccess.TryGetValue(value, out var values))
            {
                priority = values.Values.Select(x => (IQueueElement<TValue,TPriority>)x).ToList();
                return true;
            }

            priority = new List<IQueueElement<TValue, TPriority>>();
            return false;
        }


        public bool Update(int index, TValue value, TPriority priority) 
        {
            // TODO: Update value and priority at index

            return true;
        }
    }
}
