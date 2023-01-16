## TODO LIST

[ ] Code Generator for rule-sets
[X] IL gen for MapRule (obsoleted by code gen)



## PARSING
### Terminology
- Tokens:
	Parsing happens via tokens, data is turned into tokens and then those tokens are analyzed to derive and transform information.
	A 'token' is a simple conceptual structure which associates a sub-sequence of items with a type identifier, essentially making tokens a form of chunking.


### Process
Parsing processes should follow a specific pattern in order to maintain efficiency and simplicity.
#### Atomic Tokens
First, the data is transformed into simple building blocks by being 'tokenized', meaning seperated into chunks which have a simple 'type' identifier.
These building blocks should be universal concepts and should lack any kind of dynamic structure, or in other words they should be 'atomic' and unable to be reduced down into a simpler form.
Alternatively one could say that an atomic token operates exclusively by a single statement condition which is not dependent on what came before or after.
Examples of this would be the concept of a set of digits (contiguous characters in the range of 0-9), special symbol(character exactly equals '+'), or word (contiguous characters classified at 'letters').
That is, an atomic token can consume "any letters or digits" but NOT "any letters or digits IF preceeded by x character".
So an atomic token would NOT include concepts such as "a sequence of characters surrounded by quotations".

#### Compound Tokens
Second, atomic tokens are grouped into ever slightly more complex structures which represent the first level of validation.
This token is like a pattern requirement, once the pattern start is detected the token is locked in and the only possibility is that it either succeeds or fails.
Upon success the token is emitted normally, however failure results in an alternate "malformed" variant.
So each compound token should have a "malformed" variant.

#### Complex Tokens




- Tokenization: (Simple Tokens)
	Sequences of data are gathered into tokens which are a portion of the data identified as a *type* of simple token. 
	Simple tokens have only the most rudimentary of meanings and are purely a way of turning raw data into usable base components.
	Simple tokens are supposed to be abstract building blocks which can be described as simple statements of fact such as:
		- This token represents a sequence of whitespace characters.
		- This token is a number.
		- This token is an identifier of some sort, meaning text surrounded by whitespace.
		- This token represents some kind of thing whose meaning can depend on the tokens which surround it (its a building block, for example; a text symbol such as an asterisk '*' or ampersand '&')

- Interpretation: (Compound Tokens)
	Sequences of simple tokens are consumed into various *compound tokens*.
	Compound tokens actually have some meaning in the sense of parsing output, they represent an interpretation of the structure of the information without actually having knowledge of the underlying data.
	Compound tokens are much like simple tokens in that they have a type identifier and represent a sequence of values, the only difference being that the sequence of values is a collection of simple tokens.
	To be clear, compound tokens do *not* interpret data, they purely act as a mechanism for creating higher order structures from the simple tokens.
	As such, compound tokens are only aware of and consume the type identifiers of simple tokens, so a they know that the next value is 'a token of type x' but they cannot tell what data it contains at this stage.
	This

- Translation: (Complex Tokens)

