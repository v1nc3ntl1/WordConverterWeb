using System;

namespace WordConverterLibrary
{
    public class RangeException : NotSupportedException
    {
        public RangeException(string message) : base(message) {; }
    }

    public class MaxRangeException : RangeException
    {
        public MaxRangeException(string message) : base(message) {; }
    }
}
