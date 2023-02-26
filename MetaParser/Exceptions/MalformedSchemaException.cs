using System;
using System.Runtime.Serialization;

namespace MetaParser.Exceptions;

internal class MalformedSchemaException : Exception
{
    public MalformedSchemaException()
    {
    }

    public MalformedSchemaException(string message) : base(message)
    {
    }

    public MalformedSchemaException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected MalformedSchemaException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
