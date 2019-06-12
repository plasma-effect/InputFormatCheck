using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InputFormatCheck
{
    internal static class Utility
    {
        internal static IEnumerable<char> Lowers
        {
            get
            {
                for (var c = 'a'; c <= 'z'; ++c)
                {
                    yield return c;
                }
            }
        }
        internal static IEnumerable<char> Uppers
        {
            get
            {
                for (var c = 'A'; c <= 'Z'; ++c)
                {
                    yield return c;
                }
            }
        }
        private static IEnumerable<char> UnderBar
        {
            get
            {
                yield return '_';
            }
        }
        internal static SortedSet<char> NameChars { get; } = new SortedSet<char>(Lowers.Concat(Uppers).Concat(UnderBar));

        internal class LongRangeType : IEnumerable<long>
        {
            long start, count, step;

            public LongRangeType(long start, long count, long step)
            {
                this.start = start;
                this.count = count;
                this.step = step;
            }

            public IEnumerator<long> GetEnumerator()
            {
                for(var i = default(long); i < this.count; ++i)
                {
                    yield return this.start + i * this.step;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public LongRangeType Reverse()
            {
                return new LongRangeType(this.start + (this.count - 1) * this.step, this.count, -this.step);
            }
        }
        internal class IntRangeType : IEnumerable<int>
        {
            int start, count, step;

            public IntRangeType(int start, int count, int step)
            {
                this.start = start;
                this.count = count;
                this.step = step;
            }

            public IEnumerator<int> GetEnumerator()
            {
                for (var i = default(int); i < this.count; ++i)
                {
                    yield return this.start + i * this.step;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IntRangeType Reverse()
            {
                return new IntRangeType(this.start + (this.count - 1) * this.step, this.count, -this.step);
            }
        }
        internal static LongRangeType IntegerRange(long start,long end,long step = 1)
        {
            if (step == default)
            {
                throw new ArgumentException("[step] must be not zero.");
            }
            return new LongRangeType(start, (end - start) / step, step);
        }
        internal static IntRangeType IntegerRange(int start,int end,int step = 1)
        {
            if (step == 0)
            {
                throw new ArgumentException("[step] must be not zero.");
            }
            return new IntRangeType(start, (end - start) / step, step);
        }
        internal static LongRangeType IntegerRange(long end)
        {
            return new LongRangeType(default, end, 1);
        }
        internal static IntRangeType IntegerRange(int end)
        {
            return new IntRangeType(default, end, 1);
        }

        internal static List<(int Column, string Str)> Split(string str)
        {
            var ret = new List<(int Column, string Str)>();
            var s = 0;
            foreach (var i in IntegerRange(0, str.Length))
            {
                if (char.IsWhiteSpace(str[i]))
                {
                    if (i != s + 1)
                    {
                        ret.Add((s, str.Substring(s, i - s)));
                    }
                    s = i;
                }
            }
            if (str.Length != s + 1)
            {
                ret.Add((s, str.Substring(s)));
            }
            return ret;
        }

        internal static bool RangeCheck<T>(T val, T min, T max)
            where T : IComparable<T>
        {
            if (min.CompareTo(val) > 0)
            {
                return false;
            }
            if (val.CompareTo(max) > 0)
            {
                return false;
            }
            return true;
        }

    }
}
