using System;
using System.Collections.Generic;
using System.Text;

namespace InputFormatCheck
{
    [Serializable]
    public class FormatException<T> : Exception
        where T : Exception
    {
        public int Line { get; }
        public int Column { get; }
        public T Exception { get; set; }
        public FormatException(int line, int column, string message) : base($"{line + 1}, {column + 1}: {message}")
        {
            this.Line = line;
            this.Column = column;
            this.Exception = null;
        }
    }

#if DEBUG
    public
#else
    internal
# endif
        static class FormatException
    {
        static public FormatException<T> Create<T>(int line, int column, string message, T exception)
            where T : Exception
        {
            return new FormatException<T>(line, column, message)
            {
                Exception = exception
            };
        }
    }

    [Serializable]
    public class ParsingException<T> : Exception
        where T : Exception
    {
        public int Line { get; }
        public int Column { get; }
        public T Exception { get; set; }
        public ParsingException(int line, int column, string message) : base($"{line + 1}, {column + 1}: {message}")
        {

        }
    }

#if DEBUG
    public
#else
    internal
# endif
        static class PersingException
    {
        public static ParsingException<T> Create<T>(int line,int column,string message,T exception)
            where T : Exception
        {
            return new ParsingException<T>(line, column, message)
            {
                Exception = exception
            };
        }
    }
}
