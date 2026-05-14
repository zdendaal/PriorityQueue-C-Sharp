using System;
using System.Collections.Generic;
using System.Text;

namespace PriorityQueue
{
    public interface IQueueElement<TValue, TPriority>
    {
        public TValue Value { get; }
        public TPriority Priority { get; }
        public int Index { get; }
    }
}
