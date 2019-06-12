using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using static InputFormatCheck.Utility;

namespace InputFormatCheck
{
#if DEBUG
    public
# else
    internal
# endif
        interface IFormatVariable
    {
        void Check(int line, int column, string str);
    }

#if DEBUG
    public
#else
    internal
# endif
        class LongVariable : IFormatVariable
    {
        IDependentVariable min, max;
        
        public LongVariable(IDependentVariable min, IDependentVariable max)
        {
            this.min = min;
            this.max = max;
        }

        public void Check(int line, int column, string str)
        {
            if (!long.TryParse(str, out var res))
            {
                throw FormatException.Create(
                    line,
                    column,
                    "invalid string (this string must be parsed by long.TryParse)",
                    new ArgumentException());
            }
            else if (!RangeCheck(res, this.min.Value, this.max.Value))
            {
                throw FormatException.Create(
                    line,
                    column,
                    $"this value is out of range (this value is in [{this.min.Value}, {this.max.Value}]",
                    new ArgumentOutOfRangeException());
            }
        }
    }

#if DEBUG
    public
#else
    internal
#endif
        class StringVariable : IFormatVariable
    {
        IDependentVariable minLength, maxLength;
        SortedSet<char> validChars;

        public StringVariable(IDependentVariable minLength, IDependentVariable maxLength, SortedSet<char> validChars)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
            this.validChars = validChars;
        }

        public void Check(int line, int column, string str)
        {
            if (!RangeCheck(str.Length, this.minLength.Value, this.maxLength.Value))
            {
                var message = $"this string is too short or too long (this string length must be in [{this.minLength.Value}, {this.maxLength.Value}]";
                throw FormatException.Create(
                    line,
                    column,
                    message,
                    new ArgumentOutOfRangeException());
            }
            foreach(var i in IntegerRange(str.Length))
            {
                if (!this.validChars.Contains(str[i]))
                {
                    var message = "this string have invalid character";
                    throw FormatException.Create(
                        line,
                        column + i,
                        message,
                        new ArgumentException());
                }
            }
        }
    }
#if DEBUG
    public
#else
    internal
#endif
        class CharVariable : IFormatVariable
    {
        SortedSet<char> validChars;

        public CharVariable(SortedSet<char> validChars)
        {
            this.validChars = validChars;
        }

        public void Check(int line, int column, string str)
        {
            if (str.Length != 1)
            {
                var message = "this string must be a character (in other words, this string length must be 1)";
                throw FormatException.Create(
                    line,
                    column,
                    message,
                    new ArgumentException());
            }
            if (!this.validChars.Contains(str[0]))
            {
                var message = "this character is invalid";
                throw FormatException.Create(
                    line,
                    column,
                    message,
                    new InvalidDataException());
            }
        }
    }
}
