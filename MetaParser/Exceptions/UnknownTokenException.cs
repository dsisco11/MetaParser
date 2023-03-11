using System;

namespace MetaParser.Exceptions;

internal class UnknownTokenException : Exception
{
    public UnknownTokenException()
    {
    }

    public UnknownTokenException(string tokenName) : base($@"Cannot find token: ""{tokenName}""")
    {
    }
}
