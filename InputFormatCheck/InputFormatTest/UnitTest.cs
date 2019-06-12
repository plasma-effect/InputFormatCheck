using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InputFormatCheck;

namespace InputFormatTest
{
    [TestClass]
    public class UnitTest
    {
        private Exception ExceptionCheck<T0, T1, T2>(Action<T0, T1, T2> func, in T0 arg0, in T1 arg1, in T2 arg2)
        {
            try
            {
                func(arg0, arg1, arg2);
            }
            catch (Exception exp)
            {
                return exp;
            }
            return null;
        }

        [TestMethod]
        public void FormatVariableTest()
        {
            var min = new Constant(1);
            var max = new Constant(10);
            {
                var variable = new LongVariable(min, max);
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "a") is
                    FormatException<ArgumentException>);
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "0") is 
                    FormatException<ArgumentOutOfRangeException>);
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "5") is
                    null);
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "11") is
                    FormatException<ArgumentOutOfRangeException>);
            }
            {
                var variable = new StringVariable(min, max,
                    new System.Collections.Generic.SortedSet<char>
                    {
                    'a','b'
                    });
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "") is
                    FormatException<ArgumentOutOfRangeException>);
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "abababababa") is
                    FormatException<ArgumentOutOfRangeException>);
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "abc") is
                    FormatException<ArgumentException>);
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "abab") is
                    null);
            }
            {
                var variable = new CharVariable(new System.Collections.Generic.SortedSet<char>
                {
                    'a','b'
                });
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "") is
                    FormatException<ArgumentException>);
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "a") is
                    null);
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "b") is
                    null);
                Assert.IsTrue(ExceptionCheck(variable.Check, 0, 0, "c") is
                    FormatException<InvalidDataException>);
            }
        }
    }
}
