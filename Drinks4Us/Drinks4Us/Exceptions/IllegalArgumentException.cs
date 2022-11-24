using System;

namespace Drinks4Us.Exceptions
{
    public class IllegalArgumentException : ArgumentException
    {
        public IllegalArgumentException(string message) : base(message) { }
    }
}