using System;
using System.Runtime.Serialization;

namespace MetaParser.Exceptions;

internal class MetaParserException : Exception
{
    public MetaParserException()
    {
    }

    public MetaParserException(string message) : base(message)
    {
    }

    public MetaParserException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected MetaParserException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
