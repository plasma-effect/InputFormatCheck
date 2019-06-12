using System;
using System.Collections.Generic;

namespace InputFormatCheck
{
    public class InputFormat
    {
        internal class InputFormatInstance
        {
            internal List<dynamic> Constant { get; }
            internal SortedDictionary<string, dynamic> Variable { get; }
        }
    }
}
