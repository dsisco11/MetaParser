# TODO

- A consumers 'Stop' items must be mutually exclusive against the 'Consume' items
- Token processor function needs to return Token instance
- Parser should operate on a RedGreenTree
- Revamp data initialization system so that it is more flexible, structures such as tokens, consumers, and patterns should be unique, so that the graph can be more accurate
	- Patterns shouldnt automatically add themselves to the registry within their constructor
	- Pattern flattening/resolving/reducing should be done in a separate step outside of initialization

## Async Parser Mode
Generates a parser system that opereates in an asynchronous 'live' fashision.
This mode is useful for parsing large amounts of data, such as a file, or a stream.
The parser stages each operate via an enumerator, processing input and caching results to an output buffer on demand.
To an external consumer each stage appears to be a readable buffer, utilizing the SequenceReader class and a custom 'LazySequence' class.

### LazySequence
This class acts as a wrapper around a sequence of data, and provides a lazy evaluation interface.
Basically, it is a sequence of data that is evaluated on demand, and cached for future use.
It executes a given parser stage in a chunked fashion, processing a given amount of data at a time, and caching the results using ArrayPooling.
These array pools are then used to construct a SequenceReader, which is used to read the results of the parser stage.
ArrayPools are stitched together using the Sequence class.


## Custom Token Structures
Need to add a way to capture data for tokens
Consumer pattern items should be able to be given a 'field' property which contains a string name, this will cause the system to generate a custom token structure which contains a named field for each item with this property.

### Data Capture
When processing more complex stages such as those for grammer or syntax structure, it is often required that the parser capture data for immenent use.
Eg; HTML or XML tags have a starting tag, and an ending tag, and the ending tag must match the starting tag.