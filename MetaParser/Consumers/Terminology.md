# Patterns
### Constants
A pattern is _constant_ if it always results in consuming the same length of data


# Consumers
A _consumer_ operates via a set of _patterns_, detecting and consuming those patterns allows a consumer to produce a _token_.
Consumer criteria:
- Start
- Consume
- Stop
- Escape

### Constant
A consumer is considered _constant_ if it only possesses a _start_ criteria.
### Dynamic
A consumer is considered _dynamic_ if it is not _constant_, specifically if it has either a _consume_ or _stop_ criteria.
As such; the term _dynamic_ implies that a consumer will produce tokens of varying size, containing a non-predetermined amount of the data stream.
### Open
A consumer is considered _open_ if it is _dynamic_ and has no _stop_ criteria.
This implies a certain _open endedness_ for the token, and is relevant to how its code is generated but not as relevant to you as a developer.
### Closed
A consumer is considered _closed_ if it is either _constant_ OR is _dynamic_ AND has a _stop_ criteria.
This implies that the token has a definite and known stopping point.




    bool IsNext(Span<byte> stream, ETokenType type)
    {
        return type switch
        {

        };
    }