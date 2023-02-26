using System;
using System.Runtime.Serialization;

namespace MetaParser.Exceptions;

internal class PatternNotFoundException : Exception
{
    public PatternNotFoundException()
    {
    }

    public PatternNotFoundException(string message) : base(message)
    {
    }

    public PatternNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected PatternNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
