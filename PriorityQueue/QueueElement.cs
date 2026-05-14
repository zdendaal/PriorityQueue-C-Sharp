using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PriorityQueue
{
    public class QueueElement<TValue, TPriority> : IQueueElement<TValue, TPriority>
        where TValue : notnull 
        where TPriority : notnull
    {
        public TValue value;
        public TPriority priority;
        internal int index;

        public TValue Value { get { return value; }  }
        public TPriority Priority { get { return priority; }  }
        public int Index { get { return index; } }

        public QueueElement(TValue value, TPriority priority)
        {
            this.value = value;
            this.priority = priority;
        }
    }
}
