using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryHeap
{
    public class QueueElement<TValue, TPriority>
    {
        public readonly TValue value;
        public readonly TPriority priority;

        public QueueElement(TValue value, TPriority priority)
        {
            this.value = value;
            this.priority = priority;
        }
    }
}
