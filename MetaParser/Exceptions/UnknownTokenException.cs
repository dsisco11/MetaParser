using System;
using System.Runtime.Serialization;

namespace MetaParser.Exceptions;

internal class UnknownTokenException : Exception
{
    public UnknownTokenException()
    {
    }

    public UnknownTokenException(string message) : base(message)
    {
    }

    public UnknownTokenException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected UnknownTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
