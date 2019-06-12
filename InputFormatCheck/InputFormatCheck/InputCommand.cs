using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace InputFormatCheck
{
    interface IInputCommand
    {
        void Check(ref int line, TextReader reader);
    }
}
