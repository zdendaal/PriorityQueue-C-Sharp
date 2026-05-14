using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    internal class Value
    {
        public int id;
        public string name;
        public double value;

        public Value(int id, string name, double value)
        {
            this.id = id;
            this.name = name;
            this.value = value;
        }
    }
}
