using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static InputFormatCheck.Utility;

namespace InputFormatCheck
{
#if DEBUG
    public
#else
    internal
# endif
        abstract class Format
    {
        public abstract bool Check(ref int line, string str, List<Exception> exceptions);
        public List<Exception> Check(TextReader reader)
        {
            var line = 0;
            var exceptions = new List<Exception>();
            var strs = new List<string>();
            while(reader.ReadLine() is string str)
            {
                strs.Add(str);
            }
            try
            {
                Check(ref line, strs, exceptions);
            }
            catch (Exception exp)
            {
                exceptions.Add(exp);
            }
            finally
            {
                if (exceptions.Count == 0 && line < strs.Count)
                {
                    exceptions.Add(FormatException.Create(
                        line,
                        0,
                        $"this input file contains extra data (line:{line + 1} to {strs.Count})",
                        new InvalidDataException()));
                }
            }
            return exceptions;
        }
        public virtual bool Check(ref int line, List<string> strs, List<Exception> exceptions)
        {
            if (strs.Count <= line)
            {
                throw FormatException.Create(
                        line,
                        0,
                        $"reach end of input file",
                        new InvalidDataException());
            }
            return Check(ref line, strs, exceptions);
        }
    }
# if DEBUG
    public
# else
    internal
# endif
        class VariableTuple : Format
    {
        List<IFormatVariable> variables;

        public VariableTuple(List<IFormatVariable> variables)
        {
            this.variables = variables;
        }

        public override bool Check(ref int line, string str, List<Exception> exceptions)
        {
            var ar = Split(str);
            if (this.variables.Count != ar.Count)
            {
                var message = $"given arguments are few or lot (number of arguments must be {this.variables.Count})";
                exceptions.Add(FormatException.Create(
                    line,
                    0,
                    message,
                    new ArgumentOutOfRangeException()));
                ++line;
                return false;
            }
            else
            {
                foreach (var i in IntegerRange(ar.Count))
                {
                    try
                    {
                        this.variables[i].Check(line, ar[i].Column, ar[i].Str);
                    }
                    catch (Exception exp)
                    {
                        exceptions.Add(exp);
                    }
                }
            }
            ++line;
            return true;
        }
    }
#if DEBUG
    public
#else
    internal
#endif
        class VerticalArray : Format
    {
        VariableTuple tuple;
        IDependentVariable count;

        public VerticalArray(VariableTuple tuple, IDependentVariable count)
        {
            this.tuple = tuple;
            this.count = count;
        }

        public override bool Check(ref int line, string str, List<Exception> exceptions)
        {
            return this.tuple.Check(ref line, str, exceptions);
        }

        public override bool Check(ref int line, List<string> strs, List<Exception> exceptions)
        {
            foreach(var i in IntegerRange(this.count.Value))
            {
                if (strs.Count <= line)
                {
                    throw FormatException.Create(
                        line,
                        0,
                        $"reach end of input file",
                        new InvalidDataException());
                }
                if(!Check(ref line, strs[line], exceptions))
                {
                    --line;
                    exceptions.Add(FormatException.Create(
                        line,
                        0,
                        "include invalid variable tuple in vertical array",
                        new ArgumentException()));
                    return false;
                }
            }
            return true;
        }
    }

#if DEBUG
    public
#else
    internal
#endif
        class LongArray : Format
    {
        IDependentVariable min, max, count;

        public LongArray(IDependentVariable min, IDependentVariable max, IDependentVariable count)
        {
            this.min = min;
            this.max = max;
            this.count = count;
        }

        public override bool Check(ref int line, string str, List<Exception> exceptions)
        {
            var ar = Split(str);
            if (ar.Count != this.count.Value)
            {
                exceptions.Add(FormatException.Create(
                    line,
                    0,
                    $"number of array is too small or too large (the number must be in [{this.min.Value}, {this.max.Value}])",
                    new ArgumentOutOfRangeException()));
                ++line;
                return false;
            }
            var ret = true;
            foreach(var i in IntegerRange(ar.Count))
            {
                if (!long.TryParse(ar[i].Str, out var res))
                {
                    exceptions.Add(FormatException.Create(
                        line,
                        ar[i].Column,
                        "invalid string (this string must be parsed by long.TryParse)",
                        new ArgumentException()));
                    ret = false;
                }
                else if (!RangeCheck(res, this.min.Value, this.max.Value))
                {
                    exceptions.Add(FormatException.Create(
                        line,
                        ar[i].Column,
                        $"this value is out of range (this value is in [{this.min.Value}, {this.max.Value}]",
                    new ArgumentOutOfRangeException()));
                    ret = false;
                }
            }
            return ret;
        }
    }

#if DEBUG
    public
#else
    internal
#endif
        class LongDualArray : Format
    {
        IDependentVariable height;
        LongArray array;

        public LongDualArray(IDependentVariable height, IDependentVariable width, IDependentVariable min, IDependentVariable max)
        {
            this.height = height;
            this.array = new LongArray(min, max, width);
        }

        public override bool Check(ref int line, string str, List<Exception> exceptions)
        {
            return this.array.Check(ref line, str, exceptions);
        }

        public override bool Check(ref int line, List<string> strs, List<Exception> exceptions)
        {
            foreach (var i in IntegerRange(this.height.Value))
            {
                if (strs.Count <= line)
                {
                    throw FormatException.Create(
                        line,
                        0,
                        $"reach end of input file",
                        new InvalidDataException());
                }
                if (!Check(ref line, strs[line], exceptions))
                {
                    --line;
                    exceptions.Add(FormatException.Create(
                        line,
                        0,
                        "include invalid variable tuple in vertical array",
                        new ArgumentException()));
                    return false;
                }
            }
            return true;
        }
    }
#if DEBUG
    public
#else
    internal
#endif
        class CharDualArray : Format
    {

        public override bool Check(ref int line, string str, List<Exception> exceptions)
        {
            throw new NotImplementedException();
        }

        public override bool Check(ref int line, List<string> strs, List<Exception> exceptions)
        {
            return base.Check(ref line, strs, exceptions);
        }
    }
}
