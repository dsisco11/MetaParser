# MetaParser

## Description
MetaParser is a universal parsing/tokenization engine for text & binary data.   
Basically; text goes in, tokens come out.   

## Usage
The system works like so:

Create an Parser<T> instance for the data type you want to parse.   
Create a list of RuleSets for the Parser to use in chopping up the data into tokens.   
Feed the parser your data.   
It chops the data up into instances of Token (IToken) objects which each point to a small section of your data.

## Tokens
Derived from the `IToken<T>` interface.   
Tokens contain small sections of the data being parsed, and represent a logical object interpreted from said data.   
As such, a separate token is defined for each distinct concept being parsed.   
For example; code-comment tokens represent a code comment & contain the text of the comment aswell as any other relevant information required for them to be reserialized back into their raw data (the type of comment for instance; line comments prefixed with double slashes // vs block comments which span multiple lines and are surrounded by '/*' and '*/').   
This is so that tokens can be used not only for deserialization but also the other way around (serialization).   


## TokenRules
Data is transformed into tokens according to Rules.
A `TokenRule` defines how to detect and then consume/output an individual token.
For example; Whitespace sequences are defined by one rule and words or comment blocks are each defined by their own separate rules.  

However; rules are capable of being recursive, meaning that there can exist rules which, rather than consuming the data itself, consume other tokens.   
So the rules can be arranged to do something like chop the data up into more logical blocks (think words, whitespace, & code symbols), and then a series of rules can stack ontop and consume those simpler tokens into more complex structures (think function names, variable declarations, if/else blocks , etc).   

### Defaults
The library aims to come with predefined Rules for common scenarios, especially for text parsing.   
All predefined parsing rules are under the namespace `MetaParser.RuleSets`.   
For example; `MetaMod.RuleSets.Text` comes with rules for consuming text patterns such as: 
  - Newlines
  - Whitespace blocks
  - Words (contiguous letter/digit sequences)
  - Numbers
  - Code Symbols: square brackets, parenthesis, commas, etc
  - Code-Like Structures: strings(characters contained within quotations), comment blocks, etc
 
 ### Custom
 Defining your own custom parsing rules is quite easy.
 Simply create your own custom class which implements the `ITokenRule<T>` interface.
 Rules which implement this interface will intake an ITokenizer<T> object along with a reference to the previously consumed Token when their methods are called.
 Rules then provide a `Check` method which peeks at the current data using the ITokenizer<T> in order to see if the next few bits of data match the start of whatever pattern the rule is looking for.   
    
 It's important to note that rules do not check for a FULL match, instead they check for the *start* of the pattern they are consuming.   
 This is because rules should be quick to check and then, if unable to actually consume their pattern, shall produce either nothing (null), a parsing error, or a `Bad-xxx-Token` variant of their token type which contains whatever portion they _were_ able to consume.

## RuleSets
RuleSets are just a list of rules.   
However; The parsing system accepts an arbitrary number of RuleSets and so they can be combined with each other to form more complex processing.   
   
The parser processes rules recursively, meaning that all of the rules in one ruleset attempt to check/consume before the parser moves on to the next ruleset.
