using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace PriorityQueue
{
    /// <summary>
    /// Implementation of priority queueu. Heap is min-heap by default.
    /// </summary>
    /// <typeparam name="TValue">Value.</typeparam>
    /// <typeparam name="TPriority">Priority according to are elements ordered.</typeparam>
    public class CustomPriorityQueue<TValue, TPriority>
        where TValue : notnull
        where TPriority : notnull
    {

        private List<QueueElement<TValue, TPriority>> heap = new List<QueueElement<TValue, TPriority>>();

        private Dictionary<TValue, Dictionary<int, QueueElement<TValue, TPriority>>> fastAccess = new Dictionary<TValue, Dictionary<int, QueueElement<TValue, TPriority>>>();

        private readonly Comparison<TPriority> comparison;

        /// <summary>
        /// For case TPriority does implement IComparable interface.
        /// </summary>
        public CustomPriorityQueue()
        {
            comparison = Comparer<TPriority>.Default.Compare;
        }

        /// <summary>
        /// In case your TPriority does not implement IComparable interface, you must provide your own comparing method <paramref name="comparison"/>.
        /// </summary>
        /// <param name="comparison">Comparison for types that not implement IComparable interface</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CustomPriorityQueue(Comparison<TPriority> comparison) 
        {
            this.comparison = comparison ?? throw new ArgumentNullException(nameof(comparison), "Method for comparison cannot be null.");
        }

        /// <summary>
        /// Adds an element with the specified <paramref name="value"/> and <paramref name="priority"/> to the priority queue. 
        /// <paramref name="value"/> has to be unique, heap will ignore adding TValue.
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

            int valueIndex = BubbleUp(heap.Count() - 1);

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
            return !heap.Any();
        }

        public IQueueElement<TValue, TPriority> Dequeue()
        {
            if (!heap.Any())
                throw new InvalidOperationException("Queue is empty.");
            var value = heap[0];
            heap[0] = heap[heap.Count() - 1];   // insert last element at 0 index
            fastAccess[heap[0].value].Remove(heap[0].index);    // delete old index from fastAccess
            heap[0].index = 0;

            heap.RemoveAt(heap.Count() - 1);

            if (heap.Count() > 0)
            {

                int parentIndex = BubbleDown(0);

                // add element after bubbling down at key parentIndex, which is a new index where element is at.
                fastAccess[heap[parentIndex].value].Add(parentIndex, heap[parentIndex]);
            }

            // delete dequed element from fastAccess
            fastAccess[value.value].Remove(value.index);
            if (!fastAccess[value.value].Any())
                fastAccess.Remove(value.value);


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

        /// <summary>
        /// Updates element at <paramref name="index"/> position with <paramref name="value"/> and <paramref name="priority"/>.
        /// </summary>
        /// <param name="index">Index of updated element.</param>
        /// <param name="value">New value for element.</param>
        /// <param name="priority">New priority for element.</param>
        /// <returns></returns>
        public bool Update(int index, TValue value, TPriority priority) 
        {
            // throws IndexOutOfRangeException if index is out of range
            heap[index].priority = priority;
            if (fastAccess.TryGetValue(heap[index].value, out var values))  // delete element at value, index
                values.Remove(index);

            heap[index].value = value;

            int newIndex = BubbleDown(index);
            if (newIndex == index)
                newIndex = BubbleUp(index);

            // update fastAccess dictionary
            if (fastAccess.TryGetValue(value, out values))
                values.Add(newIndex, heap[newIndex]);
            else
            {
                var dict = new Dictionary<int, QueueElement<TValue, TPriority>>();
                dict.Add(newIndex, heap[newIndex]);
                fastAccess[value] = dict;
            }
            return true;
        }

        /// <summary>
        /// Bubble down element at <paramref name="index"/> to right position.
        /// </summary>
        /// <param name="index">Index of element to bubble down.</param>
        /// <returns>Index where element end up after bubble down.</returns>
        private int BubbleDown(int index)
        {
            // bubble down
            QueueElement<TValue, TPriority> buffer;
            int rightIndex = 2 * index + 2;
            int leftIndex = 2 * index + 1;
            while (rightIndex < heap.Count())
            {
                // if left node switch with current node.
                if (comparison(heap[leftIndex].priority, heap[rightIndex].priority) <= 0 && comparison(heap[leftIndex].priority, heap[index].priority) < 0)
                {
                    buffer = heap[leftIndex];
                    heap[leftIndex] = heap[index];
                    heap[index] = buffer;

                    fastAccess[buffer.value].Remove(buffer.index);
                    buffer.index = index;
                    fastAccess[buffer.value].Add(index, buffer);

                    index = leftIndex;
                    rightIndex = 2 * index + 2;
                    leftIndex = rightIndex - 1;
                }
                // if right node switch with current node.
                else if (comparison(heap[rightIndex].priority, heap[leftIndex].priority) <= 0 && comparison(heap[rightIndex].priority, heap[index].priority) < 0)
                {
                    buffer = heap[rightIndex];
                    heap[rightIndex] = heap[index];
                    heap[index] = buffer;

                    fastAccess[buffer.value].Remove(buffer.index);
                    buffer.index = index;
                    fastAccess[buffer.value].Add(index, buffer);

                    index = rightIndex;
                    rightIndex = 2 * index + 2;
                    leftIndex = rightIndex - 1;
                }
                else
                    break;
            }

            if (leftIndex == heap.Count() - 1 && comparison(heap[leftIndex].priority, heap[index].priority) < 0)
            {
                buffer = heap[leftIndex];
                heap[leftIndex] = heap[index];
                heap[index] = buffer;

                fastAccess[buffer.value].Remove(buffer.index);
                buffer.index = index;
                fastAccess[buffer.value].Add(index, buffer);

                index = leftIndex;
            }

            heap[index].index = index;
            return index;
        }

        /// <summary>
        /// Bubble up element at <paramref name="valueIndex"/>.
        /// </summary>
        /// <param name="valueIndex">Index of element to bubble up.</param>
        /// <returns>Index where element end up after bubble up.</returns>
        private int BubbleUp(int valueIndex)
        {
            // bubble up
            int parentIndex;
            QueueElement<TValue, TPriority> buffer;

            if (valueIndex % 2 == 0)
                parentIndex = (valueIndex - 2) / 2;
            else
                parentIndex = (valueIndex - 1) / 2;

            while (parentIndex >= 0)
            {
                // switch parent and child
                if (comparison(heap[valueIndex].priority, heap[parentIndex].priority) < 0)
                {
                    buffer = heap[parentIndex];
                    heap[parentIndex] = heap[valueIndex];
                    heap[valueIndex] = buffer;
                    // update index in fastAccess
                    fastAccess[buffer.value].Remove(buffer.index);
                    buffer.index = valueIndex;
                    fastAccess[buffer.value].Add(valueIndex, buffer);

                    valueIndex = parentIndex;
                    if (parentIndex % 2 == 0)
                        parentIndex = (parentIndex - 2) / 2;
                    else
                        parentIndex = (parentIndex - 1) / 2;
                }
                else
                    break;
            }

            heap[valueIndex].index = valueIndex;
            return valueIndex;
        }
    }
}
