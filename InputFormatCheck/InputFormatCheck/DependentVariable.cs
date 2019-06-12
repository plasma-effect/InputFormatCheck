using System;
using System.Collections.Generic;
using System.Text;

namespace InputFormatCheck
{
#if DEBUG
    public
#else
    internal
# endif
        interface IDependentVariable
    {
        long Value { get; }
    }

#if DEBUG
    public
#else
    internal
# endif
        class Constant : IDependentVariable
    {
        public long Value { get; }

        public Constant(long value)
        {
            this.Value = value;
        }
    }

#if DEBUG
    public
#else
    internal
# endif
        class Variable : IDependentVariable
    {
        SortedDictionary<string, long> list;
        string name;
        public long Value
        {
            get
            {
                return this.list[this.name];
            }
        }

        public Variable(SortedDictionary<string, long> list, string name)
        {
            this.list = list;
            this.name = name;
        }
    }
    
}
